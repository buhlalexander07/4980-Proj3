using UnityEngine;
using UnityEngine.AI;

public class WanderingAIComponent : MonoBehaviour
{
    public Transform herdCenter;
    public Transform barnCenter;
    [SerializeField] private float wanderRadius = 3f;
    [SerializeField] private float wanderChance = 0.1f;

    private NavMeshAgent agent;
    private AnimalRecovery recovery;
    private bool goalUpdated = false;

    private bool canMove = true;
    private bool wandering = true;
    private bool returningToHome = false;

    private Vector3 goalPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        recovery = GetComponent<AnimalRecovery>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool inBarn = false;
        if(recovery != null)
        {
            inBarn = recovery.IsRecovered;
        }
        if (wandering && !inBarn && (herdCenter.position - transform.position).magnitude > wanderRadius + 0.25f)
        {
            //Return to home state
            //We're wandering but not in the barn and outisde the home
            
            if (!returningToHome)
            {
                goalPos = herdCenter.position;
                goalUpdated = true;
                returningToHome = true;
            }
        }
        else if (wandering)
        {
            //Wandering state/Barn state
                
            //We've made it back or are in the barn if we enter this branch
            returningToHome = false;

            if(((inBarn ? barnCenter.position : herdCenter.position) - transform.position).magnitude <= wanderRadius + 0.25f)
            {
                //If we decide to randomly wander, generate a new position and set that as the destination
                if (Random.Range(0f, 1f) <= wanderChance)
                {
                    Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
                    goalPos = (inBarn ? barnCenter.position: herdCenter.position) + new Vector3(randomOffset.x, 0, randomOffset.y);
                    goalUpdated = true;
                }
            }
        }
        else if (!wandering)
        {
            //Lead state
            //We're not wandering, so we're following a target point


        }

        if (goalUpdated)
        {
            agent.destination = goalPos;
            goalUpdated = false;
        }
    }

    public void StartMoving()
    {
        agent.isStopped = false;
    }

    public void StopMoving()
    {
        agent.isStopped = true;
    }

    public void StartWandering()
    {
        wandering = true;

        //Reset returhningToHome in case we start wandering outside the home radius
        if (returningToHome)
        {
            returningToHome = false;
        }
        Debug.Log("Wandering started");
    }

    public void StartFollowingTarget()
    {
        wandering = false;
        Debug.Log("Following started");
    }

    public void SetTarget(Vector3 position)
    {
        if (!wandering)
        {
            goalPos = position;
            goalUpdated = true;
        }
    }
}
