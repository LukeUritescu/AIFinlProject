﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Timeline;
using UnityEngine;



/// <summary>
/// Have a set number of Food placements
/// If a food placement is dpeleted of resources it starts a timer and when the timer is up it becomes harvestable from Prey again
/// </summary>

   public enum PreyStates { FindFood, FoundFood, ReturnHome, Flee, FollowTrail, Dead}

public class PreySprite : MonoBehaviour
{
    public List<GameObject> ListOfFoodAvailable;

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
        this.prey = new Prey();
        curState = PreyStates.FindFood;
        hasFood = false;
        this.HP = 2;
    }

    public void TakeDamage()
    {
        this.HP--;
    }

    public void FleeFromPredator()
    {
        this.CurState = PreyStates.FindFood;
    }

    void Start()
    {
        ListOfFoodAvailable = new List<GameObject>();
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Food"))
        {
            this.ListOfFoodAvailable.Add(obj);
        }
        this.SetUpPrey();
    }

    public void SetUpPrey()
    {
        this.queen = _HiveMind.GetComponentInParent<Queen>();
        hasFood = false;
        this.curState = this.prey.State;
        if(this.queen.Hive != null)
        this.prey.Attach(queen.Hive);
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
                //if (this.GetComponentInChildren<Sight>().objectGather.activeSelf && LocationForFood == null)
                //{
                //    this.curState = PreyStates.FoundFood;
                //    LocationForFood = this.GetComponentInChildren<Sight>().objectGather.transform;
                //}
                break;
                
            //case PreyStates.FollowTrail:
            //    GoToFoodSignal(this.prey.FoodMessage);
            //    break;
            //    //This means when the prey ahs found food it will
            //case PreyStates.FoundFood:
            //    FoundFood();
            //    break;
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
        if(this.HP <= 0)
        {
            this.curState = PreyStates.Dead;
        }

        //if (this.CurState != this.prey.State && this.CurState == PreyStates.FindFood && this.prey.State == PreyStates.FollowTrail)
        //{
        //    this.CurState = this.prey.State;
        //}
    }

    public void Wandering()
    {
        this.GetComponentInChildren<Wander>().WanderAround();
    }

    public void FoundFood()
    {
            if (Vector3.Distance(LocationForFood.position, transform.position) <= 3.5f)
            {
                this.curState = PreyStates.FindFood;              
            }
        
        Quaternion tarRot = Quaternion.LookRotation(LocationForFood.position - this.transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, 5.0f * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, 20.0f * Time.deltaTime));
        
        
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (this.curState == PreyStates.FoundFood)
    //    {
    //        if(other.gameObject.tag == "Food")
    //        {

    //            if (other.gameObject.GetComponent<Food>().IsHarvestable != false)
    //            {
    //                this.curState = PreyStates.FindFood;
    //                //this.LocationForFood = null;
    //            }
    //            if (other.gameObject.GetComponent<Food>().IsHarvestable)
    //            {
    //                Debug.Log("FoundFood");
    //                this.queen.Hive.Notify(LocationForFood);
    //                this.curState = PreyStates.ReturnHome;
    //                hasFood = true;
    //                other.gameObject.GetComponent<Food>().FoodTaken();                
                    
    //            }
    //        }
    //    }
    //}

    ////This should acknowledge the food has been found so the Hivemind works and all the other prey are notified
    //public void EmitFoodLocation()
    //{

    //}

    public void GoToFoodSignal(Transform location)
    {
        if (Vector3.Distance(location.position, this.transform.position) <= distanceToHearSignal)
        {
            this.curState = PreyStates.FoundFood;
            LocationForFood = location;
        }
    }

    public void ReturnHome()
    {
        if(Vector3.Distance(HomeLocation.position, transform.position) <= 4.5f)
        {
            if (LocationForFood != null)
                this.curState = PreyStates.FoundFood;
            else
            {
                this.curState = PreyStates.FindFood;
            }
            this.hasFood = false;
            Debug.Log(this.curState);
            Debug.Log(LocationForFood);
        }
        Quaternion tarRot = Quaternion.LookRotation(HomeLocation.position - this.transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, 2.0f * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, 14.0f * Time.deltaTime));
    }

   
}
