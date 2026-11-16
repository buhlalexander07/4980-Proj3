using UnityEngine;

// Put this on anything the player can lead
public class Leadable : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3.5f;     // animal walk speed
    [SerializeField] float stopDistance = 2f;    // how far behind the player it trails

    public bool isMonster = false;

    Transform leader;      // set while being led, null when released
    bool attacking = false;


    public void StartLeading(Transform player)
    {
        if (isMonster)
        {
            // Placeholder: a monster "attacks" instead of being led.
            attacking = true;
            //Monster attack thing
            Debug.Log("Monster hit the player! (placeholder - time loss is teammate's system)");
            return;
        }
        leader = player;
    }

    public void StopLeading()
    {
        leader = null;
        attacking = false;
    }

    void Update()
    {
        if (leader == null) return;

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

