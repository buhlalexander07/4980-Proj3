using UnityEngine;


// Tests the recovery tracker's public events without depending on a game manager.
// Logs recovery progress and completion when an animal is delivered.
public class RecoveryTrackerDebugListener : MonoBehaviour
{
    [SerializeField]
    private AnimalRecoveryTracker tracker;

    /// <summary>
    /// Subscribes to the tracker's events when this listener becomes active.
    /// </summary>
    private void OnEnable()
    {
        if (tracker == null)
        {
            Debug.LogWarning("No recovery tracker assigned.");
            return;
        }

        tracker.ProgressChanged += HandleProgressChanged;
        tracker.AllRecovered += HandleAllRecovered;
    }

    /// <summary>
    /// Displays the tracker's initial state after scene initialization.
    /// </summary>
    private void Start()
    {
        if (tracker != null)
        {
            Debug.Log($"Initial recovery progress: {tracker.RecoveredCount}/{tracker.TotalAnimals}");
        }
    }

    /// <summary>
    /// Logs updated recovery progress whenever an animal is recovered.
    /// </summary>
    private void HandleProgressChanged(int recovered, int total)
    {
        Debug.Log($"Recovery progress: {recovered}/{total}");
    }

    /// <summary>
    /// Logs when every registered animal has been recovered.
    /// </summary>
    private void HandleAllRecovered()
    {
        Debug.Log("All animals recovered!");
    }

    /// <summary>
    /// Stops listening to recovery events when this test listener is disabled,
    /// preventing duplicate callbacks or outdated event subscriptions.
    /// </summary>
    private void OnDisable()
    {
        if (tracker != null)
        {
            tracker.ProgressChanged -= HandleProgressChanged;
            tracker.AllRecovered -= HandleAllRecovered;
        }
    }
}
