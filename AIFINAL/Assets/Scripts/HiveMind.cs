using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class HiveMind : ISubject
{
    List<IObserver> PreysList;

    public HiveMind()
    {
        this.PreysList = new List<IObserver>(); ;
    }
    public void Attach(IObserver o)
    {
        this.PreysList.Add(o);
    }

    public void Detach(IObserver o)
    {
        this.PreysList.Remove(o);

    }

    public void Notify()
    {
        foreach(IObserver obs in PreysList)
        {
            obs.ObserverUpdate(this, new Vector3(0, 0, 0));
        }
    }

    public void Notify(System.Object obj, Vector3 Position)
    {
        Debug.Log("Notified found food");
        foreach (IObserver obs in PreysList)
        {
            obs.ObserverUpdate(obj, Position);
        }
    }

}
