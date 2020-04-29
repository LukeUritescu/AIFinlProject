using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Timeline;
using UnityEngine;


   public enum PreyStates { FindFood, FoundFood, ReturnHome, Flee, FollowTrail}

public class PreySprite : MonoBehaviour
{


    /// <summary>
    /// create a list which adds points to it 
    /// </summary>

    [SerializeField]
    private float distanceToHearSignal = 2000f;

    [SerializeField]
    public GameObject _HiveMind;
    
    public Queen queen;

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
    private Transform LocationForFood;
    // Start is called before the first frame update

    private void Awake()
    {
        curState = PreyStates.FindFood;
        hasFood = false;
    }


    void Start()
    {
        this.prey = new Prey();
        this.SetUpPrey();
    }

    public void SetUpPrey()
    {
        queen = _HiveMind.GetComponentInParent<Queen>();
        hasFood = false;
        this.prey.Attach(queen.Hive);
        this.curState = this.prey.State;
    }

    public void DetachFromHiveMind()
    {
        this.prey.Detach(this.queen.Hive);
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
                if (this.GetComponentInChildren<Sight>().TargetToFollow != null)
                {
                    this.curState = PreyStates.FoundFood;
                    LocationForFood = this.GetComponentInChildren<Sight>().TargetToFollow;
                }
                    break;
                
            case PreyStates.FollowTrail:
                GoToFoodSignal(this.prey.FoodMessage);
                break;
                //This means when the prey ahs found food it will
            case PreyStates.FoundFood:
                FoundFood();
                break;
            case PreyStates.Flee:
                break;
            case PreyStates.ReturnHome:
                ReturnHome();
                break;
        }

        if(this.CurState != this.prey.State && this.CurState == PreyStates.FindFood && this.prey.State == PreyStates.FollowTrail)
        {
            this.CurState = this.prey.State;
        }
    }

    public void Wandering()
    {
        this.GetComponentInChildren<Wander>().WanderAround();
    }

    public void FoundFood()
    {
            if (Vector3.Distance(LocationForFood.position, transform.position) <= 2.5f)
            {
                this.queen.Hive.Notify(this, LocationForFood.position);
                this.curState = PreyStates.ReturnHome;
            hasFood = true;
            }
        Quaternion tarRot = Quaternion.LookRotation(LocationForFood.position - this.transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, 2.0f * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, 14.0f * Time.deltaTime));
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.curState == PreyStates.FindFood)
        {
            
        }
    }

    ////This should acknowledge the food has been found so the Hivemind works and all the other prey are notified
    //public void EmitFoodLocation()
    //{

    //}

    public void GoToFoodSignal(Vector3 location)
    {
        if(Vector3.Distance(location, this.transform.position) <= distanceToHearSignal)
        {
            LocationForFood.position = location;
            this.curState = PreyStates.FoundFood;
        }
    }

    public void ReturnHome()
    {
        if(Vector3.Distance(HomeLocation.position, transform.position) <= 2.5f)
        {
            this.curState = PreyStates.FindFood;
            this.hasFood = false;
        }
        Quaternion tarRot = Quaternion.LookRotation(HomeLocation.position - this.transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, 2.0f * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, 14.0f * Time.deltaTime));
    }

   
}
