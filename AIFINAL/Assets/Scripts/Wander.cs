﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    private Vector3 tarPos;

    public float MovementSpeed = 14.0f;
    private float rotSpeed = 7.0f;
    private float minX, maxX, minZ, maxZ;



    // Start is called before the first frame update
    void Start()
    {
        minX = -125.0f;
        maxX = 125.0f;

        minZ = -125.0f;
        maxZ = 125.0f;

        //GetNextPosition();
    }


    void GetNextPosition()
    {
        tarPos = new Vector3(Random.Range(minX, maxX), 2f, Random.Range(minZ, maxZ));
    }

    public void WanderAround()
    {
        if (Vector3.Distance(tarPos, transform.position) <= 5.0f)
            GetNextPosition();
        Quaternion tarRot = Quaternion.LookRotation(tarPos - transform.position);
        tarRot.eulerAngles = new Vector3(0, tarRot.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, MovementSpeed * Time.deltaTime));
    }



    // Update is called once per frame
    void Update()
    {
        
        //if (Vector3.Distance(tarPos, transform.position) <= 5.0f)
        //    GetNextPosition();
        //Quaternion tarRot = Quaternion.LookRotation(tarPos - transform.position);

        //transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);
        //transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
    }
}
