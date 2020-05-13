using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyingState { Flying, FoundSeeds, ChasePred, Scare}
public class FlyingPred : MonoBehaviour
{
    public float HearingDistance;

    public GameObject FoodLocation;

    [SerializeField]
    private float distanceToHearPredAttack;

    public GameObject GroundPredator;
    private GroundPred groundP;

    protected FlyingPredObserver observer;

    public FlyingState State;
    private void Awake()
    {
        this.observer = new FlyingPredObserver();
    }

    // Start is called before the first frame update
    void Start()
    {
        State = FlyingState.Flying;
        groundP = GroundPredator.GetComponent<GroundPred>();

        this.observer.Attach(groundP.GroundPredSub);
    }

    public void DetachFromGroundPred()
    {
        this.observer.Detach(this.groundP.GroundPredSub);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.observer.State == FlyingState.ChasePred)
            this.State = this.observer.State;
        switch (State)
        {
            case FlyingState.Flying:
                this.GetComponent<UnityFlocking>().Flocking();
                if (this.GetComponentInChildren<Sight>().DetectAspect() && !FoodLocation.GetComponent<Food>().IsHarvestable)
                {
                    this.State = FlyingState.FoundSeeds;
                    this.observer.State = FlyingState.FoundSeeds;
                }
                break;
            case FlyingState.FoundSeeds:
                FoundSeeds();
                break;
            case FlyingState.ChasePred:
                GoToPredNoise();
                break;
            case FlyingState.Scare:
                ScareThePred();
                break;
        }
    }

    public void GoToPredNoise()
    {
        Debug.Log(Vector3.Distance(this.transform.position, GroundPredator.transform.position));
        if(Vector3.Distance(this.transform.position, GroundPredator.transform.position) <= HearingDistance)
        {
            if(Vector3.Distance(this.transform.position, GroundPredator.transform.position) <= 5.0f)
            {
                this.State = FlyingState.Scare;
                this.observer.State = FlyingState.Scare;
            }
            Quaternion tarRot = Quaternion.LookRotation(GroundPredator.transform.position - transform.position);
            //tarRot.eulerAngles = new Vector3()
            transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, 20.0f * Time.deltaTime);
            transform.Translate(new Vector3(0, 80 * Time.deltaTime, 80 * Time.deltaTime));
        }
        else
        {
            this.State = FlyingState.Flying;
            this.observer.State = FlyingState.Flying;
        }
    }

    public void ScareThePred()
    {
        GroundPredator.GetComponent<GroundPred>().Scared();
        this.State = FlyingState.Flying;
        Debug.Log("Recovery!");
    }

    public void FoundSeeds()
    {
        if (Vector3.Distance(FoodLocation.transform.position, transform.position) <= 2.5f)
        {
            this.State = FlyingState.Flying;
            this.observer.State = FlyingState.Flying;
        }

        Quaternion tarRot = Quaternion.LookRotation(FoodLocation.transform.position - transform.position);
        //tarRot.eulerAngles = new Vector3()
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, 20.0f * Time.deltaTime);
        transform.Translate(new Vector3(20 * Time.deltaTime, 20 * Time.deltaTime, 20 * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(this.State == FlyingState.FoundSeeds)
        {
            if(other.gameObject.tag == "Food")
            {
                if (!other.gameObject.GetComponent<Food>().IsHarvestable)
                {
                    other.gameObject.GetComponent<Food>().FoodRestored();
                    
                }
            }
        }
    }
}
