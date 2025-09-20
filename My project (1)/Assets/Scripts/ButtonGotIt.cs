using UnityEngine;

public class ButtonGotIt : MonoBehaviour
{
    public GameObject tutorial;
    public PlayerMovement PlayerMovement; 
    public MouseMovement MouseMovement;   

    public void Start()
    {
        PlayerMovement.enabled = false;
        MouseMovement.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseTutorial()
    {
        tutorial.SetActive(false);
        PlayerMovement.enabled = true;
        MouseMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}