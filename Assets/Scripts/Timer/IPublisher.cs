using System.Collections.Generic;
using UnityEngine;

public abstract class IPublisher
{
    List<ISubscriber> subscribers;
    public void AddSubscriber(ISubscriber sub)
    {
        subscribers.Add(sub);
    }
    public void RemoveSubscriber(ISubscriber sub)
    {
        for (int i = 0; i < subscribers.Count; i++)
        {
            if (subscribers[i] == sub)
            {
                subscribers.RemoveAt(i);
                return;
            }
        }
    }
    public void NotifySubscribers()
    {
        for (int i = 0; i < subscribers.Count; i++)
        {
            subscribers[i].Notify();
        }
    }
}
