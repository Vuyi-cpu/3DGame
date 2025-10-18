using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour
{
    public CanvasGroup startButtonGroup;
    public float delay = 5f;
    public float fadeDuration = 1.5f;

    void Start()
    {
        startButtonGroup.alpha = 0f;
        startButtonGroup.gameObject.SetActive(true);
        StartCoroutine(FadeInButton());
    }

    IEnumerator FadeInButton()
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            startButtonGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        startButtonGroup.alpha = 1f;
    }
}
