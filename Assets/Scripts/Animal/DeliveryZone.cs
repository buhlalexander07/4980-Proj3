using System.Collections.Generic;
using UnityEngine;

// Defines an area where animals can be successfully delivered.
[DisallowMultipleComponent]
public class DeliveryZone : MonoBehaviour
{
    // Tracks how many colliders belonging to each animal overlap this zone.
    // This supports animals with multiple colloders without counting them twice.
    private readonly Dictionary<Leadable, int> animalsInside = new Dictionary<Leadable, int>();

    /// <summary>
    /// Called when an animal's collider enters the delivery zone.
    /// Begins tracking eligible animals and listening for their release events.
    /// </summary>
    /// <param name="other"> The collider entering the zone. </param>
    private void OnTriggerEnter(Collider other)
    {
        // Find the animal even if its collider belongs to a child object.
        Leadable animal = other.GetComponentInParent<Leadable>();

        // Only objects with both leading and recovery components qualify.
        if (animal == null || animal.isMonster || animal.GetComponent<AnimalRecovery>() == null)
        {
            return;
        }

        // An animal may have multiple colliders. Count additional overlaps
        // without subscribing to the same event more than once.
        if (animalsInside.TryGetValue(animal, out int count))
        {
            animalsInside[animal] = count + 1;
            return;
        }

        // This is the animal's first collider entering the zone.
        animalsInside.Add(animal, 1);
        animal.LeadingStopped += HandleAnimalReleased;
    }

    /// <summary>
    /// Called when an animal's collider leaves the delivery zone.
    /// Stops tracking the animal once all its colliders have exited
    /// </summary>
    /// <param name="other"> The collider leaving the zone. </param>
    private void OnTriggerExit(Collider other)
    {
        Leadable animal = other.GetComponentInParent<Leadable>();

        if (animal == null || !animalsInside.TryGetValue(animal, out int count))
        {
            return;
        }

        // Keep tracking while another collider from this animal
        // is still inside the delivery zone.
        if (count > 1)
        {
            animalsInside[animal] = count - 1;
            return;
        }

        // The animal has completely exited the zone.
        other.GetComponentInParent<AnimalRecovery>().Escape();
        animalsInside.Remove(animal);
        animal.LeadingStopped -= HandleAnimalReleased;
    }

    /// <summary>
    /// Called when an animal being tracked by this zone is released.
    /// Attempts recovery only if the animal is still inside.
    /// </summary>
    /// <param name="animal"> The animal that was released. </param>
    private void HandleAnimalReleased(Leadable animal)
    {
        // Only a release inside this zone qualifies for delivery.
        if (!animalsInside.ContainsKey(animal))
        {
            return;
        }

        AnimalRecovery recovery = animal.GetComponent<AnimalRecovery>();

        if (recovery == null)
        {
            return;
        }

        // TryRecover prevents duplicate recovery and notifies subscribers
        // only when the animal is successfully recovered for the first time.
        if (recovery.TryRecover())
        {
            Debug.Log($"{animal.name} successfully delivered to {name}!", animal);
        }
    }

    private void OnDisable()
    {
        foreach (Leadable animal in animalsInside.Keys)
        {
            if (animal != null)
            {
                animal.LeadingStopped -= HandleAnimalReleased;
            }
        }

        animalsInside.Clear();
    }
}
