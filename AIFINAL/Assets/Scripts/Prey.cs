using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Timeline;
using UnityEngine;



public class Prey : MonoBehaviour
{
    public enum PreyStates { FindFood, FoundFood, ReturnHome, Flee}

    public TrailRenderer trail;
    public GameObject TrailFollower;
    public GameObject ColliderPrefab;

    private IEnumerator coroutine;
    private float timeToMatch, timeTrail;

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
    new private Rigidbody rigidBody;

    private bool hasFood;
    private Transform LocationForFood;
    // Start is called before the first frame update

    private void Awake()
    {
        curState = PreyStates.FindFood;
        hasFood = false;
        this.trail.enabled = false;
        timeToMatch = 0.5f;
        timeTrail = 0f;
    }

    public int poolSize = 70;
    GameObject[] pool;

    void Start()
    {
        pool = new GameObject[poolSize];
        for(int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(ColliderPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeTrail += Time.deltaTime;
        switch (curState)
        {
            case PreyStates.FindFood:
                Wandering();
                if (this.GetComponentInChildren<Sight>().TargetToFollow != null)
                {
                    this.curState = PreyStates.FoundFood;
                    LocationForFood = this.GetComponentInChildren<Sight>().TargetToFollow;
                }
                StopTrail();

                    break;
            case PreyStates.FoundFood:
                FoundFood();
                StopTrail();
                break;
            case PreyStates.Flee:
                StopTrail();
                break;
            case PreyStates.ReturnHome:
                LeaveATrail();
                ReturnHome();
                break;
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
                this.curState = PreyStates.ReturnHome;
            }
        Quaternion tarRot = Quaternion.LookRotation(LocationForFood.position - this.transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, 2.0f * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, 5.0f * Time.deltaTime));
        
    }

    public void StopTrail()
    {
        if (this.trail.enabled)
            this.trail.enabled = false;

        for(int i = 0; i < pool.Length; i++)
        {
            pool[i].gameObject.SetActive(false);
        }
    }

    public void LeaveATrail()
    {
        if (this.trail.enabled == false)
            this.trail.enabled = true;
        trail.time = Vector3.Distance(LocationForFood.position, this.transform.position) * 5f;
        for(int i = 0; i < pool.Length; i++)
        {
            if(pool[i].gameObject.activeSelf == false)
            {
                if(this.timeTrail >= timeToMatch)
                {
                this.timeTrail = 0f;
                pool[i].gameObject.SetActive(true);
                pool[i].gameObject.transform.position = this.transform.position;
                coroutine = hide(trail.time, pool[i].gameObject);
                return;
                }
            }
        }
    }

    private IEnumerator hide(float waitTime, GameObject go)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            go.SetActive(false);

            yield break;
        }
        
    }

    public void ReturnHome()
    {
        if(Vector3.Distance(HomeLocation.position, transform.position) <= 2.5f)
        {
            this.curState = PreyStates.FindFood;
        }
        Quaternion tarRot = Quaternion.LookRotation(HomeLocation.position - this.transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, 2.0f * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, 5.0f * Time.deltaTime));
    }
}
