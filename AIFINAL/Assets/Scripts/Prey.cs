using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Prey : PreyFSM
{
    public enum PreyStates { Patrol, FoundFood, ReturnHome}

    public PreyStates curState;

    private float currentSpeed;

    private float currentRotationSpeed;

    private bool preyDead;

    private float minx, maxX, minZ, maxZ;

    //We overwrite the deprecated built-in'rigidbody' variable
    new private Rigidbody rigidBody;

    // Start is called before the first frame update

    protected override void Initialize()
    {
        curState = PreyStates.Patrol;
        currentSpeed = 100.0f;
        currentRotationSpeed = 2.0f;
        preyDead = false;

        GameObject objPrey = GameObject.FindGameObjectWithTag("Prey");
        preyTransform = objPrey.transform;
        rigidBody = GetComponent<Rigidbody>();

        if (!preyTransform)
            print("Prey does not exist...Please add one" + "with tag named Prey");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
