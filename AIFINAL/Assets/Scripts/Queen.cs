using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class Queen : MonoBehaviour
{
    public UnityHive Hive { get; private set; }

    private void Awake()
    {
        Hive = new UnityHive(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
