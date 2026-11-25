using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCanvas : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI cowCount;
    [SerializeField]
    private AnimalRecoveryTracker animalRecoveryTracker;
    void Start()
    {
        cowCount.text = $"Collected {animalRecoveryTracker.RecoveredCount} cows!";
    }
    public void ResetScene()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
