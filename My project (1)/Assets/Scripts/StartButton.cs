using UnityEngine;
using System.Collections;

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // <-- needed to load scenes

public class StartButton : MonoBehaviour
{
    public CanvasGroup startButtonGroup;
    public float delay = 5f;
    public float fadeDuration = 0.75f;
    public float minAlpha = 0f;
    public float maxAlpha = 1f;

    // Set your game scene name here
    public string gameSceneName = "LVL 1";

    void Start()
    {
        startButtonGroup.alpha = 0f;
        startButtonGroup.gameObject.SetActive(true);
        StartCoroutine(BlinkButton());
    }

    IEnumerator BlinkButton()
    {
        yield return new WaitForSeconds(delay);

        while (true)
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

    // This function will be called when the button is clicked
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}

