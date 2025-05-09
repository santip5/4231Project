using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static SaveManager Instance;
    public int[] passedAttacks;

    private void Awake()
    {
        Debug.Log("SaveManager Awake");
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
