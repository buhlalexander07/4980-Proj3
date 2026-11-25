using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimerNotifier : IPublisher
{
    private GlobalTimer timer;
    private bool active;
    private List<EventInterval> activeEvents = new List<EventInterval>();
    private bool sentGameOver = false;
    void Start()
    {
        if (TryGetComponent<GlobalTimer>(out timer) == false)
        {
            active = false;
        }
        else
        {
            SetActiveTimer(timer);
        }
    }
    void FixedUpdate()
    {
        if (active)
        {
            if (activeEvents != null)
            {
                for (int i = 0; i < activeEvents.Count; i++)
                {
                    if (Mathf.Abs(activeEvents[i].activationInterval - timer.GetElapsedTime()) <= Time.fixedDeltaTime)
                    {
                        NotifySubscribers(activeEvents[i].eventName);
                    }
                }
            }
            if (timer.IsGameOver() && !sentGameOver)
            {
                NotifySubscribers("gameover");
                sentGameOver = true;
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
        activeEvents.Add(new EventInterval { eventName = name, activationInterval = timeInterval });
    }
}
