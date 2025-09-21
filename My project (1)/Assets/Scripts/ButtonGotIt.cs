using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonGotIt : MonoBehaviour
{
    public GameObject tutorial;
    public PlayerMovement PlayerMovement; 
    public MouseMovement MouseMovement;
    public GameObject buttonfirst;

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