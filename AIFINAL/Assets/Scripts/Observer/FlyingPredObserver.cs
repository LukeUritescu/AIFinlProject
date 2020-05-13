using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPredObserver : IObserver
{
    protected FlyingState _state;

    public FlyingState State
    {
        get { return _state; }
        set
        {
            if(this._state != value)
            {
                this._state = value;
            }
        }
    }

    public FlyingPredObserver(GroundPredSubject sub)
    {
        this.State = FlyingState.Flying;
        sub.Attach(this);
    }

    public FlyingPredObserver()
    {
        this.State = FlyingState.Flying;
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
        if(sender is GroundPredSubject)
        {
            if(message is string)
            {
                switch (message.ToString())
                {
                    case "Attack":
                        this.State = FlyingState.ChasePred;
                        break;
                }
            }
        }
    }
}
