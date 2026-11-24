using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPublisher : MonoBehaviour
{
    protected List<ISubscriber> subscribers;
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
