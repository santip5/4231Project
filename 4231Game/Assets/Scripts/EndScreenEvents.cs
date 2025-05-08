using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class EndScreenEvents : MonoBehaviour
{
    private UIDocument _endScreenDoc;

    private Button _restart;
    private Button _menu;

    private void Awake()
    {
        _endScreenDoc = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _endScreenDoc.rootVisualElement;

        _restart = root.Q("RestartButton") as Button;
        _restart.RegisterCallback<ClickEvent>(RestartClick);


        _menu = root.Q("MenuButton") as Button;
        _menu.RegisterCallback<ClickEvent>(MenuClick);
    }

    private void OnDisable()
    {
        _restart.UnregisterCallback<ClickEvent>(RestartClick);
        _menu.UnregisterCallback<ClickEvent>(MenuClick);
    }

    private void RestartClick(ClickEvent evt)
    {
        Debug.Log("You pressed the restart button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f; // Resume the game
    }

    private void MenuClick(ClickEvent evt)
    {
        Debug.Log("You pressed the main menu button");
        Time.timeScale = 1f; // Ensure the game is unpaused before switching scenes
        SceneManager.LoadScene("MainMenu");
    }


}
