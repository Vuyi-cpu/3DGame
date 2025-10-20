using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;

public class CutsceneController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "LVL 1";
    public TextMeshProUGUI skipText;
    public float skipTextDuration = 3f;

    private bool canSkip = false;

    void Start()
    {
        
        videoPlayer.Play();

        
        skipText.gameObject.SetActive(true);
        Invoke(nameof(HideSkipText), skipTextDuration);

        
        videoPlayer.loopPointReached += OnVideoEnd;

        
        Invoke(nameof(EnableSkip), 1f);
    }

    void Update()
    {
        if (canSkip && Input.anyKeyDown)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    void HideSkipText()
    {
        skipText.gameObject.SetActive(false);
    }

    void EnableSkip()
    {
        canSkip = true;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
