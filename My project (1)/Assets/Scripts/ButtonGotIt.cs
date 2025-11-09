using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonGotIt : MonoBehaviour
{
    public GameObject tutorial;
    public GameObject katanaTutorial;
    public GameObject scytheTutorial;
    public GameObject shopTutorial;
    public GameObject stunTutorial; //ASSIGN IN INSPECTOR
    public PlayerMovement PlayerMovement; 
    public MouseMovement MouseMovement;
    public GameObject buttonfirst;
    public bool katanaActive;
    public bool scytheActive;
    public bool shopActive;
    public bool stunActive;
    PlayerState playerlife;
    public GameObject deathScreen;
    public AudioSource uisound;
    static bool  controlTut = false;
    public void Start()
    {
        if (controlTut)
        {
            PlayerMovement.enabled = true;
            MouseMovement.enabled = true;

        }
        else
        {
            PlayerMovement.enabled = false;
            MouseMovement.enabled = false;
        }
        playerlife = FindFirstObjectByType<PlayerState>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(buttonfirst);
    }

    public void CloseTutorial()
    {
        controlTut = true;
        uisound.Play();
        if (katanaActive) Destroy(katanaTutorial);
        if (scytheActive) Destroy(scytheTutorial);
        if (shopActive) Destroy(shopTutorial);
        if (stunActive) Destroy(stunTutorial);
        if (playerlife.currentHealth<=0) deathScreen.SetActive(true);
        Time.timeScale = 1f;
        tutorial.SetActive(false);
        PlayerMovement.enabled = true;
        MouseMovement.enabled = true;
        PlayerMovement.pauseactive = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EventSystem.current.SetSelectedGameObject(null);
    }
}