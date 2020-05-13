using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPredSubject : ISubject
{
    List<IObserver> Listeners;

    protected GroundPredStates _state;

    public GroundPredStates State
    {
        get { return _state; }
        set
        {
            if(_state != value)
            {
                _state = value;
            }
        }
    }

    public GroundPredSubject()
    {
        this.Listeners = new List<IObserver>();
        this.State = GroundPredStates.VehicleFollow;
    }

    public void Attach(IObserver o)
    {
        this.Listeners.Add(o);
    }

    public void Detach(IObserver o)
    {
        this.Listeners.Remove(o);

        int indexOfFirstDeadListener = 0;
        foreach(Prey g in Listeners)
        {
            if(g.State == PreyStates.Dead)
            {
                indexOfFirstDeadListener = Listeners.IndexOf(g);
                this.Listeners.RemoveAt(indexOfFirstDeadListener);
            }
        }
        
    }

    public void Notify()
    {
       foreach(IObserver o in Listeners)
        {
            o.ObserverUpdate(this, "Message from Ground Predator");
        }
    }

    public void Notify(string s)
    {
        foreach(IObserver o in Listeners)
        {
            o.ObserverUpdate(this, s);
        }
    }

}
