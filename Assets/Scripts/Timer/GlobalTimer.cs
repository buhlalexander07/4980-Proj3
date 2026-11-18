using TMPro;
using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    [SerializeField] private float default_time;
    private float target_time;
    [SerializeField] private float curr_time;
    private bool paused;
    private bool completed;
    [SerializeField] private TextMeshProUGUI timer_text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if (target_time < 0)
        //{
        //    target_time = default_time;
        //}
        target_time = default_time;
        curr_time = 0;
        paused = false;
        completed = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimerDisplay();   
        if (!paused)
        {
            curr_time += Time.deltaTime;
            if (curr_time >= target_time)
            {
                paused = true;
            }
        }
        if (curr_time >= target_time)
        {
            completed = true;
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
    public bool GetTimerStatus()
    {
        return completed;
    }
    public void ResetTimer()
    {
        curr_time = 0;
        completed = false;
    }

    public void AddTime()
    {
        curr_time += 30.0f;
    }

    private void UpdateTimerDisplay()
    {
        if (timer_text != null)
        {
            if (completed)
            {
                timer_text.text = "TIME UP";
            }
            else
            {
                timer_text.text = curr_time.ToString("F2") + "s";
            }
        }
    }
}
