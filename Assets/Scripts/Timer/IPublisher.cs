using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPublisher : MonoBehaviour
{
    // Used AI to debug null references
    protected List<ISubscriber> subscribers = new List<ISubscriber>();
    public void AddSubscriber(ISubscriber sub)
    {
        subscribers.Add(sub);
    }
    public void RemoveSubscriber(ISubscriber sub)
    {
        subscribers.Remove(sub);
    }
    protected void NotifySubscribers(String info)
    {
        for (int i = 0; i < subscribers.Count; i++)
        {
            subscribers[i].Notify(info);
        }
    }
}
