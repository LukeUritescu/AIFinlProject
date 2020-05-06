using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityFlocking : MonoBehaviour
{
    public float minSpeed = 20.0f;
    public float turnSpeed = 20.0f;
    public float randomFreq = 20.0f;

    public float randomForce = 20.0f;
    public float toOriginForce = 20.0f;
    public float toOriginRange = 100.0f;

    public float gravity = 2.0f;

    public float avoidanceRadius = 4.0f;
    public float avoidanceForce = 20.0f;

    public float followVelocity = 4.0f;
    public float followRadius = 40.0f;

    private Transform origin;
    private Vector3 velocity;
    private Vector3 normalizedVelocity;
    private Vector3 randomPush;
    private Vector3 originPush;
    private Transform[] objects;
    private UnityFlocking[] otherFlocks;
    private Transform transformComponent;



    // Start is called before the first frame update
    void Start()
    {
        randomFreq = 1.0f / randomFreq;

        //assin the parent as origin
        origin = transform.parent;

        transformComponent = transform;

        //Temporary components
        Component[] tempFlocks = null;

        //Get all the unity flock components from the parent transform in the group
        if (transform.parent)
        {
            tempFlocks = transform.parent.GetComponentsInChildren<UnityFlocking>();

        }

        //Assign and store all the flock objects in this g roup
        objects = new Transform[tempFlocks.Length];
        otherFlocks = new UnityFlocking[tempFlocks.Length];

        for(int i = 0; i < tempFlocks.Length; i++)
        {
            objects[i] = tempFlocks[i].transform;
            otherFlocks[i] = (UnityFlocking)tempFlocks[i];
        }

        //Null Parent as the flock leader will be UnityFlockController object
        transform.parent = null;

        //Calculate random push depends on the random frequency provided
        StartCoroutine(UpdateRandom());


    }

    IEnumerator UpdateRandom()
    {
        while(true)
        {
            randomPush = Random.insideUnitSphere * randomForce;
            yield return new WaitForSeconds(randomFreq + Random.Range(-randomFreq / 2.0f, randomFreq / 2.0f));
        }
    }

    public void Flocking()
    {
        float speed = velocity.magnitude;
        Vector3 avgVelocity = Vector3.zero;
        Vector3 avgPosition = Vector3.zero;
        float count = 0;
        float f = 0.0f;
        float d = 0.0f;
        Vector3 myPosition = transformComponent.position;
        Vector3 forceV;
        Vector3 toAvg;
        Vector3 wantedVel;

        for (int i = 0; i < objects.Length; i++)
        {
            Transform transform = objects[i];
            if (transform != transformComponent)
            {
                Vector3 otherPosition = transform.position;

                avgPosition += otherPosition;
                count++;

                forceV = myPosition - otherPosition;
                d = forceV.magnitude;

                if (d < followRadius)
                {
                    if (d < avoidanceRadius)
                    {
                        f = 1.0f - (d / avoidanceRadius);
                        if (d > 0)
                        {
                            avgVelocity += (forceV / d) * f * avoidanceForce;
                        }
                    }

                    //just keep the current distance with the leader
                    f = d / followRadius;
                    UnityFlocking tempOtherFlock = otherFlocks[i];
                    avgVelocity += tempOtherFlock.normalizedVelocity * f * followVelocity;
                }
            }
        }

        if (count > 0)
        {
            avgVelocity /= count;

            toAvg = (avgPosition / count) - myPosition;
        }
        else
        {
            toAvg = Vector3.zero;
        }

        forceV = origin.position - myPosition;
        d = forceV.magnitude;
        f = d / toOriginRange;

        if (d > 0)
            originPush = (forceV / d) * f * toOriginForce;
        if (speed < minSpeed && speed > 0)
        {
            velocity = (velocity / speed) * minSpeed;
        }

        wantedVel = velocity;

        //Caluclate final velocity
        wantedVel -= wantedVel * Time.deltaTime;
        wantedVel += randomPush * Time.deltaTime;
        wantedVel += originPush * Time.deltaTime;
        wantedVel += avgVelocity * Time.deltaTime;
        wantedVel += toAvg.normalized * gravity * Time.deltaTime;

        //Final velocity to rotate the flock into
        velocity = Vector3.RotateTowards(velocity, wantedVel, turnSpeed * Time.deltaTime, 100.00f);
        transformComponent.rotation = Quaternion.LookRotation(velocity);

        transformComponent.Translate(velocity * Time.deltaTime, Space.World);

        normalizedVelocity = velocity.normalized;
    }
// Update is called once per frame
void Update()
{
}
}



