using UnityEngine;

// Coordinates animal movement between autonomous wandering and player leading.
// Ensures that only one movement system controls the animal at a time.
[DisallowMultipleComponent]
[RequireComponent(typeof(Leadable), typeof(Rigidbody))]
public class AnimalMovementCoordinator : MonoBehaviour
{
    private Leadable leadable;
    private WanderingAIComponent wanderingAI;
    private AnimalRecovery recovery;
    private Rigidbody rb;

    // Remember the original movement settings so they can be restored.
    private bool wasKinematic;
    private bool wanderingWasEnabled;

    /// <summary>
    /// Finds the components responsible for leading, wandering, recovery,
    /// and physics on this animal.
    /// </summary>
    private void Awake()
    {
        leadable = GetComponent<Leadable>();
        wanderingAI = GetComponent<WanderingAIComponent>();
        recovery = GetComponent<AnimalRecovery>();
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Begins listening for changes in the animal's leading and recovery state.
    /// Also handles animals that are already being led or have been recovered.
    /// </summary>
    private void OnEnable()
    {
        leadable.LeadingStarted += HandleLeadingStarted;
        leadable.LeadingStopped += HandleLeadingStopped;

        if (recovery != null)
        {
            recovery.Recovered += HandleRecovered;
        }

        // Synchronize movement if this component is enabled mid-game.
        if (recovery != null && recovery.IsRecovered)
        {
            FreezeRecoveredAnimal();
        }
        else if (leadable.IsBeingLed)
        {
            HandleLeadingStarted(leadable);
        }
    }

    /// <summary>
    /// Stops listening when this component is disabled or destroyed,
    /// preventing outdated event subscriptions.
    /// </summary>
    private void OnDisable()
    {
        leadable.LeadingStarted -= HandleLeadingStarted;
        leadable.LeadingStopped -= HandleLeadingStopped;

        if (recovery != null)
        {
            recovery.Recovered -= HandleRecovered;
        }
    }

    /// <summary>
    /// When the player starts leading, pauses autonomous wandering and
    /// transfers movement control to Alex's leading system.
    /// </summary>
    private void HandleLeadingStarted(Leadable animal)
    {
        if (recovery != null && recovery.IsRecovered)
        {
            return;
        }

        // save the pre-leading settings for restoration on release.
        wasKinematic = rb.isKinematic;
        wanderingWasEnabled = wanderingAI != null && wanderingAI.enabled;

        PauseWandering();
        FreezePhysics();
    }

    /// <summary>
    /// When the player releases the animal, restores normal wandering
    /// unless the animal has already been recovered
    /// </summary>
    private void HandleLeadingStopped(Leadable animal)
    {
        if (recovery != null && recovery.IsRecovered)
        {
            FreezeRecoveredAnimal();
            return;
        }

        // Restore the animal's original physics mode.
        rb.isKinematic = wasKinematic;

        // prevent leftover momentum from moving the released animal.
        if (!rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // resume wandering only if it was enabled before leading began.
        if (wanderingAI != null && wanderingWasEnabled)
        {
            wanderingAI.enabled = true;
            wanderingAI.StartWandering();
        }
    }

    /// <summary>
    /// When recovery succeeds, stops any remaining leading behavior and
    /// permanently disables autonomous movement so the animal stays in place.
    /// </summary>
    private void HandleRecovered(AnimalRecovery animal)
    {
        // Ensure the animal stops following, even if recovery is triggered
        // by another system before the player manually releases it.
        if (leadable.IsBeingLed)
        {
            leadable.StopLeading();
        }

        // keep the recovered animal stationary.
        FreezeRecoveredAnimal();
    }

    /// <summary>
    /// Stops Greg's wandering behavior and prevents its Update method
    /// from randomly restarting movement during leading.
    /// </summary>
    private void PauseWandering()
    {
        if (wanderingAI == null)
        {
            return;
        }

        wanderingAI.StopWandering();
        wanderingAI.enabled = false;
    }

    /// <summary>
    /// Stops physics-driven motion while another system controls position.
    /// Kinematic Rigidbodies can be moved without responding to forces.
    /// </summary>
    private void FreezePhysics()
    {
        // Unity doesn't allow setting velocity on a kinemetic Rigidbody I think.
        if (!rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        rb.isKinematic = true;
    }

    /// <summary>
    /// Finalizes a recovered animal's movement state by disabling wandering
    /// and keeping its Rigidbody under non-physics control.
    /// </summary>
    private void FreezeRecoveredAnimal()
    {
        PauseWandering();
        FreezePhysics();
    }
}
