using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Timers;
using UnityEngine;

    public enum GroundPredStates { VehicleFollow, Chase, Attack, Recovery, Dead}
public class GroundPred : MonoBehaviour
{
    private float elapsedTime;
    public float SetTimerForRecoveryAfterAttack;
    
    public UnityGroundPredator GroundPredSub { get; private set; }

    private int hp = 3;
    [SerializeField]
    public int HP
    {
        get
        {
            return this.hp;
        }
        set
        {
            if(this.hp != value)
            {
                this.hp = value;
            }
        }
    }

    public GameObject PreyReference;


    [SerializeField]
    private GroundPredStates states;
    public GroundPredStates States
    {
        get { return this.states; }
        set
        {
            if(this.states != value)
            {
                this.states = value;
            }
        }
    }

    private Transform PreyLocation;

    private void Awake()
    {
        GroundPredSub = new UnityGroundPredator(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        states = GroundPredStates.VehicleFollow;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Prey")
        {
            this.states = GroundPredStates.Chase;
            this.GroundPredSub.Notify("Attack");
            this.PreyReference = coll.gameObject;
        }
    }

    //public void GatherResource()
    //{
    //    //if(Vector3.Distance(foodLocation.position, transform.position) <= 2.5f)
    //    //{
    //    //    this.states = GroundPredStates.VehicleFollow;
    //    //}
    //    Quaternion tarRot = Quaternion.LookRotation(PreyLocation.position - this.transform.position);
    //    transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, 10.0f * Time.deltaTime);
    //    transform.Translate(new Vector3(0, 0, 30.0f * Time.deltaTime));
    //}

    // Update is called once per frame
    void Update()
    {
        switch (states)
        {
            case GroundPredStates.VehicleFollow:
                this.GetComponent<VehicleFollow>().FollowPath();
                break;
            case GroundPredStates.Chase:
                Chasing();
                break;
            case GroundPredStates.Attack:
                Attacking();
                break;
            case GroundPredStates.Recovery:
                Recovery();
                elapsedTime += Time.deltaTime;
                break;
            case GroundPredStates.Dead:
                this.gameObject.SetActive(false);
                this.gameObject.GetComponent<BoxCollider>().enabled = false;
                break;
        }
        this.transform.position = new Vector3(this.transform.position.x, 3, this.transform.position.z);
        if(this.HP <= 0)
        {
            this.states = GroundPredStates.Dead;
        }
    }

    //This is for chasing the prey when predator 
    public void Chasing()
    {
        if (Vector3.Distance(PreyReference.transform.position, transform.position) <= 3.5f)
        {
            this.GroundPredSub.Notify("Attack");
            this.states = GroundPredStates.Attack;
        }
        Quaternion tarRot = Quaternion.LookRotation(PreyReference.transform.position - this.transform.position);
        tarRot.eulerAngles = new Vector3(0, tarRot.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, 10.0f * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, 37.0f * Time.deltaTime));
    }

    public void Scared()
    {
        this.elapsedTime = 0f;
        this.States = GroundPredStates.Recovery;
    }

    public void Recovery()
    {
        if(elapsedTime >= SetTimerForRecoveryAfterAttack)
        {
            this.PreyReference = null;
            this.states = GroundPredStates.VehicleFollow;
            elapsedTime = 0f;
        }
    }

    //Should attack the prey when in range
    public void Attacking()
    {
        if(this.PreyReference != null)
        {
            this.PreyReference.GetComponent<PreySprite>().FleeFromPredator();
            this.PreyReference.GetComponent<PreySprite>().TakeDamage();
            this.states = GroundPredStates.Recovery;
            
        }
    }
}
