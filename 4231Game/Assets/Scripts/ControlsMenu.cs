using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ControlsMenu : MonoBehaviour
{
    private VisualElement root;
    private DropdownField slot1;
    private DropdownField slot2;
    private DropdownField slot3;
    private Button testButton;
    private Button confirmButton;
    private ControlsDummy playerAttackSequence;

    private List<string> moveOptions = new List<string> { "Attack 1", "Attack 2", "Attack 3" };

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        playerAttackSequence = FindFirstObjectByType<ControlsDummy>();

        slot1 = root.Q<DropdownField>("Slot1");
        slot2 = root.Q<DropdownField>("Slot2");
        slot3 = root.Q<DropdownField>("Slot3");
        testButton = root.Q<Button>("TestButton");
        confirmButton = root.Q<Button>("ConfirmButton");
        confirmButton.RegisterCallback<ClickEvent>(ConfirmClick);

        slot1.choices = new List<string>(moveOptions);
        slot2.choices = new List<string>(moveOptions);
        slot3.choices = new List<string>(moveOptions);

        slot1.value = moveOptions[0];
        slot2.value = moveOptions[1];
        slot3.value = moveOptions[2];

        testButton.clicked += () =>
        {
            int attack1 = slot1.index;
            int attack2 = slot2.index;
            int attack3 = slot3.index;

            Debug.Log($" Sending order: {attack1}, {attack2}, {attack3}");

            playerAttackSequence.SetAttackOrder(attack1, attack2, attack3);
            playerAttackSequence.StartAttackSequence();
        };
    }

    private void ConfirmClick(ClickEvent evt)
    {
        Debug.Log("You pressed the confirm button");
        SceneManager.LoadScene("PlayerTest");
    }
}