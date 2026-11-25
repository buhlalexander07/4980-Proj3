using System;
using System.Collections.Generic;
using UnityEngine;

// Tracks recovery progress across a group of animals.
// Announces progress and completion without depending on a specific game manager.
[DisallowMultipleComponent]
public class AnimalRecoveryTracker : IPublisher
{
    // Animals already placed in the scene can be assigned through the Inspector.
    // Animals spawned at runtime can be added through RegisterAnimal().
    [SerializeField]
    private List<AnimalRecovery> startingAnimals = new List<AnimalRecovery>();

    // A HashSet prevents the same animal from being registered twice.
    private readonly HashSet<AnimalRecovery> trackedAnimals = new HashSet<AnimalRecovery>();

    public int TotalAnimals => trackedAnimals.Count;
    public int RecoveredCount { get; private set; }

    // An empty group does not count as a completed objective.
    public bool AreAllRecovered => TotalAnimals > 0 && RecoveredCount == TotalAnimals;

    // Reports the number recovered and the total number of animals.
    public event Action<int, int> ProgressChanged;

    // Announces when the last registered animal is recovered.
    public event Action AllRecovered;

    /// <summary>
    /// Registers animals assigned in the Inspector when the tracker initializes.
    /// Additional animals may be registered later by a spawning system.
    /// </summary>
    private void Awake()
    {
        foreach (AnimalRecovery animal in startingAnimals)
        {
            RegisterAnimal(animal);
        }
    }

    /// <summary>
    /// Adds an animal to recovery tracking and listens for its recovery event.
    /// Animals already recovered when registered are counted immediately.
    /// Duplicate registrations are ignored.
    /// </summary>
    /// <param name="animal"> The Animal to track. </param>
    /// <returns> True if registered; false if null or already registered. </returns>
    public bool RegisterAnimal(AnimalRecovery animal)
    {
        // Ignore invalid references and animals already being tracked.
        if (animal == null || !trackedAnimals.Add(animal))
        {
            return false;
        }

        // Account for animals recovered before registration.
        if (animal.IsRecovered)
        {
            RecoveredCount++;
        }

        // Listen for future recovery events from this animal.
        animal.Recovered += HandleAnimalRecovered;

        // Notify listeners that the total population has changed.
        ProgressChanged?.Invoke(RecoveredCount, TotalAnimals);

        return true;
    }

    /// <summary>
    /// Called when a registered animal is successfully recovered.
    /// Updates the recovery count and announces progress or completion.
    /// </summary>
    /// <param name="animal"> The newly recovered animal. </param>
    private void HandleAnimalRecovered(AnimalRecovery animal)
    {
        // Only registered animals contribute to the recovery objective.
        if (!trackedAnimals.Contains(animal))
        {
            return;
        }

        // AnimalRecovery.TryRecover() fires its event only once,
        // so each registered animal contributes at most one recovery.
        RecoveredCount++;
        NotifySubscribers("recovered");

        // Notify listeners before checking whether recovery is complete.
        ProgressChanged?.Invoke(RecoveredCount, TotalAnimals);

        if (AreAllRecovered)
        {
            AllRecovered?.Invoke();
        }
    }

    /// <summary>
    /// Removes event subscriptions when the tracker is destroyed,
    /// preventing references to a tracker that no longer exists.
    /// </summary>
    private void OnDestroy()
    {
        foreach (AnimalRecovery animal in trackedAnimals)
        {
            if (animal != null)
            {
                animal.Recovered -= HandleAnimalRecovered;
            }
        }
    }
}
