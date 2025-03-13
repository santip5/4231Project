using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class PauseMenuEvents : MonoBehaviour
{
    private UIDocument _pauseDocument;

    private Button _continue;
    private Button _menu;

    public GameObject PauseMenu;

    private void Awake()
    {
        _pauseDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _pauseDocument.rootVisualElement;

        _continue = root.Q("ContinueButton") as Button;
        _continue.RegisterCallback<ClickEvent>(ContinueClick);


        _menu = root.Q("BackButton") as Button;
        _menu.RegisterCallback<ClickEvent>(MenuClick);
    }

    private void OnDisable()
    {
        _continue.UnregisterCallback<ClickEvent>(ContinueClick);
        _menu.UnregisterCallback<ClickEvent>(MenuClick);
    }

    private void ContinueClick(ClickEvent evt)
    {
        Debug.Log("You pressed the continue button");
        PauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    private void MenuClick(ClickEvent evt)
    {
        Debug.Log("You pressed the main menu button");
        Time.timeScale = 1f; // Ensure the game is unpaused before switching scenes
        SceneManager.LoadScene("MainMenu");
    }


}
