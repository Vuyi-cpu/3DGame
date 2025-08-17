using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMovement : MonoBehaviour
{
    public float mouseSens = 100f;
    public float controllerSens = 300f; 

    PlayerControls controls;
    private Vector2 rotate;

    float xLook = 0f;
    float yLook = 0f;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.look.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Player.look.canceled += ctx => rotate = Vector2.zero;
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void Start()
    {
        //Locking the cursor to the middle of the screen and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        bool isMouse = false;

        if (controls.Player.look.activeControl != null)
        {
           isMouse = controls.Player.look.activeControl.device is Mouse;// detect what device is giving input
        }

        float sens;
        float deltaMultiplier;


        if (isMouse)
        {
            sens = mouseSens;
            deltaMultiplier = 1f;
        }
        else
        {
            sens = controllerSens;
            deltaMultiplier = Time.deltaTime;
        }

        float mouseX = rotate.x * sens * deltaMultiplier;
        float mouseY = rotate.y * sens * deltaMultiplier;
       
       

        //control rotation around x axis (Look up and down)
        xLook -= mouseY;

        //we clamp the rotation so we cant Over-rotate (like in real life)
        xLook = Mathf.Clamp(xLook, -90f, 90f);

        //control rotation around y axis (Look up and down)
        yLook += mouseX;

        //applying both rotations
        transform.localRotation = Quaternion.Euler(xLook, yLook, 0f);
    }
}






