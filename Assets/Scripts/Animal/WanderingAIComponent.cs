using UnityEngine;

public class WanderingAIComponent : MonoBehaviour
{
    public Transform herdCenter;
    [SerializeField] private float wanderRadius = 3f;

    [SerializeField] private float chanceToStopWandering = 0.001f;
    [SerializeField] private float chanceToStartWandering = 0.01f;

    [SerializeField] private float maxMoveSpeed = 0.5f;
    [SerializeField] private float moveAccel = 5f;

    [SerializeField] private float chanceToChangeAngle = 0.001f;
    [SerializeField] private float maxTurningPerFrame = 0.01f;

    private Rigidbody rb;

    private bool canWander = true;
    private bool currentlyWandering = true;

    private Vector3 targetVector = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canWander && Random.Range(0f, 1f) < chanceToStopWandering || !canWander && Random.Range(0f, 1f) < chanceToStartWandering)
        {
            canWander = !canWander;
        }

        if (canWander && currentlyWandering)
        {
            #region Rotation Handling
            // If outside the radius, set target angle to be center of herd
            Vector3 currHorizontalPos = new Vector3(rb.position.x, 0, rb.position.z);
            if((herdCenter.position - rb.position).magnitude > wanderRadius)
            {
                Vector3 targetHorizontalPos = new Vector3(herdCenter.position.x, 0, herdCenter.position.z);
                targetVector = (targetHorizontalPos - currHorizontalPos).normalized;
            }
            else if(Random.Range(0f, 1f) < chanceToChangeAngle)
            {
                Debug.Log("Random Angle Change");
                targetVector = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            }
            Vector3 rotateVector = Vector3.RotateTowards(transform.forward, targetVector, maxTurningPerFrame, 0.0f);
            Vector3 rotateVectorHorizontal = new Vector3(rotateVector.x, 0, rotateVector.z).normalized;
            rb.rotation = Quaternion.Euler(0, Quaternion.LookRotation(rotateVectorHorizontal).eulerAngles.y, 0);
            #endregion

            #region Forward Movement
            if (rb.linearVelocity.magnitude < maxMoveSpeed)
            {
                rb.AddForce(transform.forward * moveAccel, ForceMode.Acceleration);
            }
            #endregion
        }
        Debug.DrawLine(rb.position, rb.position + targetVector, Color.red, 0.1f);
    }

    public void StartWandering()
    {
        canWander = true;
    }

    public void StopWandering()
    {
        canWander = false;
    }
}
