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
    public AudioSource uitutsound;
    public AudioSource uiclosesound;

    void Awake()
    {
        
        MouseMovement = FindFirstObjectByType<MouseMovement>();
        PlayerMovement= FindFirstObjectByType<PlayerMovement>();
    }

    public void Resume()
    {
        uiclosesound.Play();
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
        uitutsound.Play();
        deathScreen.SetActive(false);
        tutorial.SetActive(true);
        pause.SetActive(false);
    }
  
}
