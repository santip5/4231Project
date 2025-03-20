using UnityEngine;
using Unity.Cinemachine;

public class CinemachineViewportAdjust : MonoBehaviour
{
    private Camera mainCamera;
    private CinemachineBrain cinemachineBrain;

    void Start()
    {
        mainCamera = Camera.main;
        cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();

        if (mainCamera != null && cinemachineBrain != null)
        {
            AdjustViewport();
        }
    }

    void AdjustViewport()
    {
        mainCamera.rect = new Rect(0.5f, 0f, 0.5f, 1f);
    }
}
