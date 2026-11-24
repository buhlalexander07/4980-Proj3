using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimerNotifier : IPublisher
{
    private GlobalTimer timer;
    private bool active;
    private List<EventInterval> activeEvents;
    void Start()
    {
        if (TryGetComponent<GlobalTimer>(out timer) == false)
        {
            active = false;
        }
    }
    void FixedUpdate()
    {
        if (active)
        {
            for (int i = 0; i < activeEvents.Count; i++)
            {
                if (Mathf.Abs(activeEvents[i].activationInterval - timer.GetElapsedTime()) <= Time.fixedDeltaTime)
                {
                    NotifySubscribers(activeEvents[i].eventName);
                }
            }
        }
    }

    public void SetActiveTimer(GlobalTimer target)
    {
        timer = target;
        active = true;
    }

    private struct EventInterval
    {
        public String eventName;
        public float activationInterval;
    }
    
    public void addEventInterval(String name, float timeInterval)
    {
        activeEvents.Add(new EventInterval {eventName = name, activationInterval = timeInterval});
    }
}
