using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : IObserver
{
    protected PreyStates state;

    protected Transform foodMessage;
    public Transform FoodMessage
    {
        get;set;
    }

    public PreyStates State
    {
        get { return state; }
        set
        {
            if(this.state != value)
            {
                this.state = value;
            }
        }
    }

    public Prey(HiveMind hm)
    {
        this.state = PreyStates.FindFood;
        hm.Attach(this);
    }

    public Prey()
    {
        this.state = PreyStates.FindFood;
    }

    public void Attach(HiveMind hm)
    {
        hm.Attach(this);
    }

    public void Detach(HiveMind hm)
    {
        hm.Detach(this);
    }

    public void ObserverUpdate(object sender, Transform message)
    {
        if (sender is HiveMind)
        {
            //If this prey has NOT found food yet then this will help them find it if they are within range
           if(this.State == PreyStates.FindFood)
            {
                this.State = PreyStates.FollowTrail;
                this.FoodMessage = message;
            }
        }
    }
}
