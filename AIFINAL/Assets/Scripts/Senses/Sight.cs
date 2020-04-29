using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sight : Sense
{
    public int FieldOfView = 45;
    public int ViewDistance = 10;

    private Vector3 rayDirection;

    [SerializeField]
    private string[] aspectNameArray;
    private List<GameObject> objectsToSearch;
    private Transform targetToFollow;

    public Transform TargetToFollow
    {
        get { return this.targetToFollow; }
        set
        {
            if(this.targetToFollow != value)
            {
                this.targetToFollow = value;
            }
        }
    }

    /// <summary>
    /// I want this to be reusable for each ai type that wants to use it. As of right now
    /// the code only supports for one object to search for ONE other object
    /// I should make it so ANY AI can use this script
    /// Should take advantage of AsepctName 
    /// At start up, find each object with the tag in the arrayAspectNameArray
    /// </summary>
    protected override void Initialize()
    {
          objectsToSearch = new List<GameObject>();
        foreach(string name in aspectNameArray)
        {

            foreach(GameObject obj in GameObject.FindGameObjectsWithTag(name))
            {
                objectsToSearch.Add(obj);
            }
        }

    }

    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= detectionRate) AllTargets();
    }

    private void AllTargets()
    {
        foreach(GameObject obj in objectsToSearch.ToList())
        {
            if (obj.activeSelf)
            {
                DetectAspect(obj.transform);
                //Debug.DrawLine(transform.position, obj.transform.position, Color.blue);
            }
            else if(obj.activeSelf == false)
            {
                this.objectsToSearch.Remove(obj);
                this.TargetToFollow = null;   
            }

        }
    }

    public void DetectAspect(Transform targetTransform) 
    {
        RaycastHit hit;
       
        rayDirection = targetTransform.position - transform.position;
        //Check the angle between the AI characters forward vector and the dirrection vector between pllayer and AI
        if ((Vector3.Angle(rayDirection, transform.forward)) < FieldOfView)
        {
            //Detect if player is withhin the field of view
            if (Physics.Raycast(transform.position, rayDirection, out hit, ViewDistance))
            {
                Aspect aspect = hit.collider.GetComponent<Aspect>();
                if (aspect != null)
                {
                    //Check the aspect
                    if (aspect.aspectName == aspectName)
                    {
                        //print(aspectName.ToString());
                        TargetToFollow = targetTransform;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isEditor)
            return;

        //Debug.DrawLine(transform.position, foodTransform.position, Color.red);

        Vector3 frontRayPoint = transform.position + (transform.forward * ViewDistance);

        Vector3 leftRayPoint = Quaternion.Euler(0, FieldOfView * 0.5f, 0) * frontRayPoint;

        Vector3 rightRayPoint = Quaternion.Euler(0, -FieldOfView * 0.5f, 0) * frontRayPoint;

        Debug.DrawLine(transform.position, frontRayPoint, Color.green);
        Debug.DrawLine(transform.position, leftRayPoint, Color.green);
        Debug.DrawLine(transform.position, rightRayPoint, Color.green);
    }

}
