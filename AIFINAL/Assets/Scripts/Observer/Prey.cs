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

    public Prey(GroundPredSubject sub)
    {
        this.state = PreyStates.FindFood;
        sub.Attach(this);
    }

    public Prey()
    {
        this.state = PreyStates.FindFood;
    }

    public void Attach(GroundPredSubject sub)
    {
        sub.Attach(this);
    }

    public void Detach(GroundPredSubject sub)
    {
        sub.Detach(this);
    }

    public void ObserverUpdate(object sender, object message)
    {
        if (sender is GroundPredSubject)
        {
           if(message is string)
            {
                switch (message.ToString())
                {
                    case "Attack":
                        this.State = PreyStates.Flee;
                        break;
                }
            }
        }
    }
}
