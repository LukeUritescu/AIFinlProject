using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFollow : MonoBehaviour
{
    public Path path;
    public float Speed = 60.0f;
    public float mass = 5.0f;
    public bool isLooping = true;

    private float curSpeed;

    private int curPathIndex;
    private float pathLength;
    private Vector3 targetPoint;

    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        pathLength = path.Length;
        curPathIndex = 0;

        velocity = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FollowPath()
    {
        curSpeed = Speed * Time.deltaTime;
        targetPoint = path.GetPoint(curPathIndex);

        if (Vector3.Distance(transform.position, targetPoint) < path.Radius)
        {
            if (curPathIndex < pathLength - 1)
            {
                curPathIndex++;
            }
            else if (isLooping)
                curPathIndex = 0;
            else
                return;
        }

        if (curPathIndex >= pathLength)
            return;
        if (curPathIndex >= pathLength - 1 && !isLooping)
            velocity += Steer(targetPoint, true);
        else
            velocity += Steer(targetPoint);

        Vector3 dirRot = targetPoint - transform.position;
        Quaternion tarRotation = Quaternion.LookRotation(dirRot);
        tarRotation.eulerAngles = new Vector3(0, tarRotation.eulerAngles.y, 0);


        transform.rotation = Quaternion.LookRotation(velocity);
        transform.position += velocity;
    }

    public Vector3 Steer(Vector3 target, bool bFinalPoint = false)
    {
        Vector3 desiredVelocity = (target - transform.position);
        float dist = desiredVelocity.magnitude;

        desiredVelocity.Normalize();

        if (bFinalPoint && dist < 10.0f)
            desiredVelocity *= (curSpeed * (dist / 10.0f));
        else
            desiredVelocity *= curSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;
        Vector3 acceleration = steeringForce / mass;

        return acceleration;
    }
}
