using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.Collections;

public class PortraitCameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 localOffset = new Vector3(0, 1.9f, 0.5f); 
    public bool lookAtTarget = true;

    public RawImage portraitImage;
    public float fadeDuration = 1f;


    private IEnumerator FadeColorRoutine()
    {
        Color start = Color.white;
        Color target = new Color32(105, 105, 105, 255);

        float time = 0f;
        while (time < fadeDuration)
        {
            portraitImage.color = Color.Lerp(start, target, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        portraitImage.color = target;
    }

    private void Start()
    {
        PlayerController.OnPlayerDied += DeadCamera;
    }
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 worldOffset = target.TransformDirection(localOffset);
        transform.position = target.position + worldOffset;

        if (lookAtTarget)
        {
            transform.LookAt(target.position + Vector3.up * 1.5f); 
        }
    }

    private void DeadCamera()
    {
        localOffset = new Vector3(0, -0.3f, 0.7f);
        StartCoroutine(FadeColorRoutine());
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerDied -= DeadCamera;
    }
}