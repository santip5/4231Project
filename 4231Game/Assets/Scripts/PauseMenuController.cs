using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject PauseMenu; // Assign your UI element or any GameObject in the inspector
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Change to your desired key
        {
            ToggleMenu();
        }
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
}