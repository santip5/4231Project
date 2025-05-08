using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject LoseScreen;
    public GameObject WinScreen;
    private bool isPaused = false;


    private void Start()
    {
        PlayerController.OnPlayerDied += Lose;
        EnemyLogic.OnEnemyDied += Win;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Change to your desired key
        {
            ToggleMenu();
        }
    }
    private void Lose()
    {
        LoseScreen.SetActive(true);
    }

    private void Win()
    {
        WinScreen.SetActive(true);
    }

    void ToggleMenu()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1f; // Resume the game
        }
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerDied -= Lose;
        EnemyLogic.OnEnemyDied -= Win;
    }
}