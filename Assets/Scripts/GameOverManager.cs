using Unity.VisualScripting;
using UnityEngine;

public class GameOverManager : ISubscriber
{
    // Used AI to help figure out how to subscribe to events properly
    [SerializeField]
    private TimerNotifier notifier;
    [SerializeField]
    private GameOverCanvas gameOverCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        notifier.AddSubscriber(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Used AI to figure out how to override the notify for a simpler usage
    public override void Notify(string info)
    {
        if (info == "gameover")
        {
            Debug.Log("recieved game over");
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameOverCanvas.gameObject.SetActive(true);
        }
    }

}
