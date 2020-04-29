using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : IObserver
{
    protected PreyStates state;

    protected Vector3 foodMessage;
    public Vector3 FoodMessage
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

    public void ObserverUpdate(object sender, Vector3 message)
    {
        if (sender is Prey)
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
