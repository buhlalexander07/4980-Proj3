using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    [SerializeField] private float default_time;
    private float target_time;
    private float curr_time;
    private bool paused;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (target_time < 0)
        {
            target_time = default_time;
        }
        curr_time = 0;
        paused = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            curr_time += Time.deltaTime;
            if (curr_time > target_time)
            {
                paused = true;
            }
        }
    }

    public void StopTimer()
    {
        paused = true;
    }

    public void StartTimer()
    {
        paused = false;
    }

    public void SetTargetTime(float new_time)
    {
        target_time = new_time;
    }

    public float GetElapsedTime()
    {
        return curr_time;
    }
}
