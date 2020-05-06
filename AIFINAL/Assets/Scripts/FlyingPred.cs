using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPred : MonoBehaviour
{
    public enum FlyingState { Flying, FoundSeeds, ReturnSeedsToHome}

    public FlyingState State;
    // Start is called before the first frame update
    void Start()
    {
        State = FlyingState.Flying;   
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case FlyingState.Flying:
                break;
            case FlyingState.FoundSeeds:
                break;
            case FlyingState.ReturnSeedsToHome:
                break;
        }
    }
}
