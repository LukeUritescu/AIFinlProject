using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyFSM : MonoBehaviour
{

    protected Transform preyTransform;

    protected Vector3 destPosition;

    

    protected virtual void Initialize() { }

    protected virtual void FSMUpdate() { }

    protected virtual void FSMFixedUpdate() { }

    

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        FSMUpdate();
    }

    private void FixedUpdate()
    {
        FSMFixedUpdate();
    }
}
