using UnityEngine;
using System.Collections;

public class PlayerCam : MonoBehaviour
{
    public Camera cam;
    Coroutine fovRoutine;

    public void DoFov(float targetFov, float duration = 0.25f)
    {
        if (fovRoutine != null) StopCoroutine(fovRoutine);
        fovRoutine = StartCoroutine(LerpFov(targetFov, duration));
    }

    private IEnumerator LerpFov(float targetFov, float duration)
    {
        float start = cam.fieldOfView;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            cam.fieldOfView = Mathf.Lerp(start, targetFov, t);
            yield return null;
        }

        cam.fieldOfView = targetFov;
        fovRoutine = null;
    }
}
