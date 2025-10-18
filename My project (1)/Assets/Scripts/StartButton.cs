using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour
{
    public CanvasGroup startButtonGroup;
    public float delay = 5f;
    public float fadeDuration = 0.75f; // how fast it fades in/out
    public float minAlpha = 0f;        // lowest alpha during blink
    public float maxAlpha = 1f;        // highest alpha during blink

    void Start()
    {
        startButtonGroup.alpha = 0f;
        startButtonGroup.gameObject.SetActive(true);
        StartCoroutine(BlinkButton());
    }

    IEnumerator BlinkButton()
    {
        // Initial delay before blinking starts
        yield return new WaitForSeconds(delay);

        while (true) // infinite blinking
        {
            // Fade in
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                startButtonGroup.alpha = Mathf.Lerp(minAlpha, maxAlpha, elapsed / fadeDuration);
                yield return null;
            }

            // Fade out
            elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                startButtonGroup.alpha = Mathf.Lerp(maxAlpha, minAlpha, elapsed / fadeDuration);
                yield return null;
            }
        }
    }
}
