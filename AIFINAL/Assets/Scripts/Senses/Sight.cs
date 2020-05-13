using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : Sense
{
    public int FieldOfView = 45;
    public int ViewDistance = 100;

    private Transform playerTransform;
    private Vector3 rayDirection;

    public string Target;

    protected override void Initialize()
    {
        playerTransform = GameObject.FindGameObjectWithTag(Target).transform;
    }

    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= detectionRate) DetectAspect();
    }

    public bool DetectAspect()
    {
        RaycastHit hit;

        rayDirection = playerTransform.position - transform.position;
        float angle = Vector3.Angle(rayDirection, transform.forward);
        if (angle < FieldOfView)
        {
            if (Physics.Raycast(transform.position, rayDirection, out hit, ViewDistance))
            {
                Aspect aspect = hit.collider.GetComponent<Aspect>();
                if (aspect != null)
                {
                    if (aspect.aspectName == aspectName)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isEditor || playerTransform == null)
        {
            return;
        }

        //Debug.DrawLine(transform.position, playerTransform.position, Color.red);
        Vector3 frontRayPoint = transform.position + (transform.forward * ViewDistance);

        Vector3 leftRayPoint = Quaternion.Euler(0, FieldOfView, 0) * frontRayPoint;

        Vector3 rightRayPoint = Quaternion.Euler(0, -FieldOfView, 0) * frontRayPoint;

        Debug.DrawLine(transform.position, frontRayPoint, Color.green);
        Debug.DrawLine(transform.position, leftRayPoint, Color.blue);
        Debug.DrawLine(transform.position, rightRayPoint, Color.cyan);
    }
}
