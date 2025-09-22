using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonGotIt : MonoBehaviour
{
    public GameObject tutorial;
    public GameObject katanaTutorial;
    public GameObject scytheTutorial;
    public GameObject shopTutorial;
    public PlayerMovement PlayerMovement; 
    public MouseMovement MouseMovement;
    public GameObject buttonfirst;
    public bool katanaActive;
    public bool scytheActive;
    public bool shopActive;

    public void Start()
    {
        PlayerMovement.enabled = false;
        MouseMovement.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(buttonfirst);
    }

    public void CloseTutorial()
    {
        if (katanaActive) Destroy(katanaTutorial);
        if (scytheActive) Destroy(scytheTutorial);
        if (shopActive) Destroy(shopTutorial);
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