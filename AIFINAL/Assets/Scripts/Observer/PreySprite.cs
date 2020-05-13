using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;



/// <summary>
/// Have a set number of Food placements
/// If a food placement is dpeleted of resources it starts a timer and when the timer is up it becomes harvestable from Prey again
/// </summary>

   public enum PreyStates { FindFood, FoundFood, ReturnHome, Flee, FollowTrail, Dead}

public class PreySprite : MonoBehaviour
{
    
    public GameObject FoodLocation;
    [SerializeField]
    private int hp;
    public int HP
    {
        get { return this.hp; }
        set
        {
            if(this.hp != value)
            {
                this.hp = value;
            }
        }
    }

    /// <summary>
    /// create a list which adds points to it 
    /// </summary>

    [SerializeField]
    private float distanceToHearSignal = 2000f;

    public GameObject GroundPredator;
    private GroundPred groundP;

    private Prey prey;

   [SerializeField]
    private PreyStates curState;
    public PreyStates CurState
    {
        get { return curState; }
        set
        {
            if(this.curState != value)
            {
                this.curState = value;
            }
        }
    }

    public Transform HomeLocation;

    //We overwrite the deprecated built-in'rigidbody' variable
    //new private Rigidbody rigidBody;

    private bool hasFood;

    // Start is called before the first frame update

    private void Awake()
    {
        this.prey = new Prey();
        curState = PreyStates.FindFood;
        hasFood = false;
        this.HP = 2;
    }

    public void TakeDamage()
    {
        this.HP--;
    }


    void Start()
    {
        this.SetUpPrey();
    }

    public void SetUpPrey()
    {
        groundP = GroundPredator.GetComponent<GroundPred>();

        this.prey.Attach(groundP.GroundPredSub);
        hasFood = false;
        this.curState = this.prey.State;

    }

    public void DetachFromGroundPred()
    {
        this.prey.Detach((this.groundP.GroundPredSub));
    }

    // Update is called once per frame
    void Update()
    {
        //timeTrail += Time.deltaTime;
        switch (curState)
        {
            //The Prey should be searching for food, if it has seen food it should then switch to FoundFood State
            case PreyStates.FindFood:
                Wandering();
                if (this.GetComponentInChildren<Sight>().DetectAspect() && this.FoodLocation.GetComponent<Food>().IsHarvestable)
                {
                    this.CurState = PreyStates.FoundFood;
                    //Debug.Log("found food");
                }
                else
                {
                    this.CurState = PreyStates.FindFood;
                }
                break;

            case PreyStates.FollowTrail:
                GoToFoodSignal(this.prey.FoodMessage);
                break;
            //This means when the prey ahs found food it will
            case PreyStates.FoundFood:
                FoundFood();
                break;
            case PreyStates.ReturnHome:
                ReturnHome();
                break;
            case PreyStates.Flee:
                FleeFromPredator();
                break;
            case PreyStates.Dead:
                this.gameObject.SetActive(false);
                this.gameObject.GetComponent<BoxCollider>().enabled = false;
                break;
        }
        this.transform.position = new Vector3(this.transform.position.x, 2, this.transform.position.z);
        if(this.HP <= 0)
        {
            this.curState = PreyStates.Dead;
        }

        //if (this.CurState != this.prey.State && this.CurState == PreyStates.FindFood && this.prey.State == PreyStates.FollowTrail)
        //{
        //    this.CurState = this.prey.State;
        //}
    }
    public void FleeFromPredator()
    {
        this.CurState = PreyStates.FindFood;
    }

    public void Wandering()
    {
        this.GetComponentInChildren<Wander>().WanderAround();
    }

    public void FoundFood()
    {
            if (Vector3.Distance(FoodLocation.transform.position, transform.position) <= 4.5f)
            {
                this.curState = PreyStates.ReturnHome;             
            }
        
        Quaternion tarRot = Quaternion.LookRotation(FoodLocation.transform.position - this.transform.position);
        tarRot.eulerAngles = new Vector3(0, tarRot.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, 10.0f * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, 40.0f * Time.deltaTime));
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.curState == PreyStates.FoundFood)
        {
            if (other.gameObject.tag == "Food")
            {
                if (other.gameObject.GetComponent<Food>().IsHarvestable && this.hasFood == false)
                {
                    other.gameObject.GetComponent<Food>().FoodTaken();
                    this.hasFood = true;
                    //Debug.Log("has food");
                }
                else
                {
                    this.hasFood = false;
                    this.CurState = PreyStates.FindFood;
                }
            }
        }
    }

    ////This should acknowledge the food has been found so the Hivemind works and all the other prey are notified
    //public void EmitFoodLocation()
    //{

    //}

    public void GoToFoodSignal(Transform location)
    {
        if (Vector3.Distance(location.position, this.transform.position) <= distanceToHearSignal)
        {
            this.curState = PreyStates.FoundFood;
        }
    }

    public void ReturnHome()
    {
        if(Vector3.Distance(HomeLocation.position, transform.position) <= 4.5f)
        {
            if (FoodLocation.gameObject.GetComponent<Food>().IsHarvestable)
                this.curState = PreyStates.FoundFood;
            else
            {
                this.curState = PreyStates.FindFood;
            }
            this.hasFood = false;
        }
        Quaternion tarRot = Quaternion.LookRotation(HomeLocation.position - this.transform.position);
        tarRot.eulerAngles = new Vector3(0, tarRot.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, 10.0f * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, 40.0f * Time.deltaTime));
    }

   
}
