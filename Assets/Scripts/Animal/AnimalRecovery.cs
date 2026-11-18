using System;
using UnityEngine;

// Tracks whether an animal has been successfully recovered.
// Once recovered, the animal cannot be recovered again, and other systems are notified.
[DisallowMultipleComponent]
public class AnimalRecovery : MonoBehaviour
{
    // other components can check recovery status but cannot directly change it.
    public bool IsRecovered { get; private set; }

    // Subscribers are notified when this animal is recovered for the first time.
    public event Action<AnimalRecovery> Recovered;

    /// <summary>
    /// Called when another system determines that this animal qualifies for recovery.
    /// If the animal hasn't been recovered, marks it as recovered and notifies listeners.
    /// Otherwise, leaves its state unchanged.
    /// </summary>
    /// <returns> True if newly recoered; false if already recovered. </returns>
    public bool TryRecover()
    {
        // prevent duplicate recoveries and notifications.
        if (IsRecovered)
        {
            return false;
        }

        // update state before notifying listeners to prevent repeated recovery.
        IsRecovered = true;

        // notify subscribers, passing this component so they know whic animal was recovered.
        // null-conditional operator handles zero subscribers.
        Recovered?.Invoke(this);

        return true;
    }
}
