using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISubscriber : MonoBehaviour
{
    protected List<GameEvent> gameEvents;
    public virtual void Notify(String info)
    {
        for (int i = 0; i < gameEvents.Count; i++)
        {
            gameEvents[i].ActivateEvent(info);
        }
    }

    void AddEvent(GameEvent gameEvent) {
        gameEvents.Add(gameEvent);
    }
}
