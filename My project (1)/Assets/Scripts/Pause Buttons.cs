using UnityEngine;
using static UnityEngine.Timeline.DirectorControlPlayable;

public class PauseButtons : MonoBehaviour
{
    public GameObject pause;
    MouseMovement MouseMovement;
    PlayerMovement PlayerMovement;
    public GameObject tutorial;

    void Awake()
    {
        MouseMovement = FindFirstObjectByType<MouseMovement>();
        PlayerMovement= FindFirstObjectByType<PlayerMovement>();
    }

    public void Resume()
    {
        MouseMovement.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        pause.SetActive(false);
        PlayerMovement.pauseactive = false;
        Time.timeScale = 1f;

    }
   public void Controls()
    {
        tutorial.SetActive(true);
        pause.SetActive(false);

    }
  
}
