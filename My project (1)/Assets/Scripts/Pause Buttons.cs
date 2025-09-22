using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Timeline.DirectorControlPlayable;

public class PauseButtons : MonoBehaviour
{
    public GameObject pause;
    MouseMovement MouseMovement;
    PlayerMovement PlayerMovement;
    public GameObject tutorial;
    public GameObject deathScreen;

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
        EventSystem.current.SetSelectedGameObject(null);
    }
   public void Controls()
    {
        deathScreen.SetActive(false);
        tutorial.SetActive(true);
        pause.SetActive(false);
    }
  
}
