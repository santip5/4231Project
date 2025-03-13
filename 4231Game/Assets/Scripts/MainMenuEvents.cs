using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;

    private Button _newRun;
    private Button _quit;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _newRun = _document.rootVisualElement.Q("NewRunButton") as Button;
        _newRun.RegisterCallback<ClickEvent>(NewRunClick);


        _quit = _document.rootVisualElement.Q("QuitGameButton") as Button;
        _quit.RegisterCallback<ClickEvent>(QuitClick);
    }

    private void OnDisable()
    {
        _newRun.UnregisterCallback<ClickEvent>(NewRunClick);
    }

    private void NewRunClick(ClickEvent evt)
    {
        Debug.Log("You pressed the new run button");
        SceneManager.LoadScene("MenuTest");
    }

    private void QuitClick(ClickEvent evt)
    {
        Debug.Log("You pressed the quit button");
        Application.Quit();
    }


}
