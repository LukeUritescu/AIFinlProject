using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFSM : FSM
{
    public enum FSMState { None, Patrol, Chase, Strike, Dead }

    public FSMState state;

    private float curSpeed;

    private float curRotSpeed;

    private bool bDead;
    private int health;

    new private Rigidbody rigidBody;

    protected override void Initialize()
    {
        state = FSMState.Patrol;
        curSpeed = 80.0f;
        curRotSpeed = 2.0f;
        bDead = false;
        health = 2;

        pointList = GameObject.FindGameObjectsWithTag("WanderPoint");

        FindNextPoint();

        GameObject objPrey = GameObject.FindGameObjectWithTag("Prey");
        preyTransform = objPrey.transform;
        rigidBody = GetComponent<Rigidbody>();

        if (!preyTransform)
            print("Prey doesn't exist...Please add one" + " with Tag Named Prey");


    }

    protected override void FSMUpdate()
    {
        switch (state)
        {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
            case FSMState.Strike: UpdateAttackState(); break;
            case FSMState.Dead: UpdateDeadState(); break;
        }
    }

    protected void UpdatePatrolState()
    {
        if(Vector3.Distance(transform.position, destPos) <= 100.0f)
        {
            print("Reached to the destination point\n" + "calculating the next point");
            FindNextPoint();
        }

        else if (Vector3.Distance(transform.position, preyTransform.position) <= 300.0f)
        {
            print("Switch to chase position");
            state = FSMState.Chase;
        }

        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

    protected void FindNextPoint()
    {
        print("finding next point");
        int rndIndex = Random.Range(0, pointList.Length);
        float rndRadius = 10.0f;
        Vector3 rndPosition = Vector3.zero;
        destPos = pointList[rndIndex].transform.position + rndPosition;

        if (IsInCurrentRange(destPos))
        {
            rndPosition = new Vector3(Random.Range(-rndRadius, rndRadius), 0.0f, Random.Range(-rndRadius, rndRadius));
            destPos = pointList[rndIndex].transform.position + rndPosition;
        }
    }

    protected bool IsInCurrentRange(Vector3 pos)
    {
        float xPos = Mathf.Abs(pos.x - transform.position.x);
        float zPos = Mathf.Abs(pos.z - transform.position.z);
        if (xPos <= 50 && zPos <= 50)
            return true;
        return false;
    }

    protected void UpdateChaseState()
    {
        destPos = preyTransform.position;

        float dist = Vector3.Distance(transform.position, preyTransform.position);

        if(dist <= 200.0f)
        {
            state = FSMState.Strike;
        }

        else if (dist>= 300.0f)
        {
            state = FSMState.Patrol;
        }

        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

    protected void UpdateAttackState()
    {
        destPos = preyTransform.position;

        float dist = Vector3.Distance(transform.position, preyTransform.position);
        if(dist <= 100.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);
            transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);

            state = FSMState.Strike;
        }

        else if (dist >= 200)
        {
            state = FSMState.Patrol;
        }

        
    }

    protected void UpdateDeadState()
    {
        if (!bDead)
        {
            bDead = true;
            this.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "FlyingPred")
        {
            health -= 1;
        }
    }


}
