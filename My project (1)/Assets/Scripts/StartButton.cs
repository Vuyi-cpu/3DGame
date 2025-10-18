using UnityEngine;

public class StartButton : MonoBehaviour
{
    public GameObject startButton;
    public float delay = 5f;

    void Start()
    {
        startButton.SetActive(false);
        Invoke(nameof(ShowButton), delay);
    }

    void ShowButton()
    {
        startButton.SetActive(true);
    }
}
