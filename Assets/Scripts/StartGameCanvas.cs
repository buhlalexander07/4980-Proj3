using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameCanvas : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Starting game");
        SceneManager.LoadScene("main");
    }
}
