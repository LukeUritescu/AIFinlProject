using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GroundPred : MonoBehaviour
{


    public enum GroundPredStates { Wander, VehicleFollow, GoToFoodRemains, RetrieveFoodRemains}

    private GroundPredStates states;

    private Transform foodLocation;

    // Start is called before the first frame update
    void Start()
    {
        states = GroundPredStates.VehicleFollow;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Food")
        {
            if(coll.gameObject.GetComponent<Food>().harvestState == Food.HarvestState.HarvestRemains)
            {
                Debug.Log("Hooray");
                foodLocation = coll.gameObject.transform;
                this.states = GroundPredStates.GoToFoodRemains;

            }
        }
    }

    public void GatherResource()
    {
        if(Vector3.Distance(foodLocation.position, transform.position) <= 3.5f)
        {
            this.states = GroundPredStates.VehicleFollow;

        }
        Quaternion tarRot = Quaternion.LookRotation(foodLocation.position - this.transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, 2.0f * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, 20.0f * Time.deltaTime));
    }

    // Update is called once per frame
    void Update()
    {
        switch (states)
        {
            case GroundPredStates.VehicleFollow:
                this.GetComponent<VehicleFollow>().FollowPath();
                break;
            case GroundPredStates.GoToFoodRemains:
                GatherResource();
                break;
        }
    }
}
