using System;
using UnityEngine;

// Put this on anything the player can lead
// Handles leading state, movement, and notifications when leading starts or stops.
public class Leadable : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3.5f;     // animal walk speed
    [SerializeField] float stopDistance = 2f;    // how far behind the player it trails

    public bool isMonster = false;

    // Other components can observe when this animal starts or stops following.
    public event Action<Leadable> LeadingStarted;
    public event Action<Leadable> LeadingStopped;

    // Lets other components check whether this animal is currently being led
    public bool IsBeingLed => leader != null;

    // Recovered animals can no longer be interacted with.
    // Objects without a recovery component remain interactable
    public bool CanInteract => recovery == null || !recovery.IsRecovered;

    // Recovery is optional because monsters and other interactable objects
    // may use Leadable without being recoverable.
    private AnimalRecovery recovery;

    Transform leader;      // set while being led, null when released
    bool attacking = false;

    /// <summary>
    /// Finds this object's optional recovery component so future interactions
    /// can check whether the animal has already been delivered
    /// </summary>
    private void Awake()
    {
        recovery = GetComponent<AnimalRecovery>();
    }

    /// <summary>
    /// Called when the player interacts with this animal to begin leading it.
    /// Monsters trigger their attack behavior instead of following the player.
    /// </summary>
    /// <param name="player"> The Transform of the player leading this animal. </param>
    public void StartLeading(Transform player)
    {
        // A recovered animal is no longer eligible for interaction.
        // Check here as well as in the player controllers so other callers
        // cannot accidentally restart leading.
        if (!CanInteract)
        {
            return;
        }

        if (isMonster)
        {
            // A monster "attacks" instead of being led.
            attacking = true;
            gameObject.GetComponent<Monster>().Attack();
            Debug.Log("Monster hit the player! (placeholder - time loss is teammate's system)");
            return;
        }

        // avoid repeated start notifications for an animal already being led.
        if (leader != null)
        {
            return;
        }

        // begin following the player, then notify interested components
        leader = player;
        LeadingStarted?.Invoke(this);
    }

    /// <summary>
    /// Called when the player releases the animal.
    /// Stops following and notifies other components that leading has ended.
    /// </summary>
    public void StopLeading()
    {
        // Only announce a release if the animal was actually being led.
        bool wasBeingLed = leader != null;

        // Clear the current leader and preserve the existing attack reset.
        leader = null;
        attacking = false;

        // Other components can now check whether the animal was delivered.
        if (wasBeingLed)
        {
            LeadingStopped?.Invoke(this);
        }
    }

    void Update()
    {
        if (leader == null)
        {
            return;
        }

        // Follow behind stopDistance away.
        Vector3 target = leader.position - leader.forward * stopDistance;
        target.y = transform.position.y; 

        Vector3 toTarget = target - transform.position;
        if (toTarget.magnitude > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, target, moveSpeed * Time.deltaTime);
        }
    }
}

