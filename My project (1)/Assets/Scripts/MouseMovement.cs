using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMovement : MonoBehaviour
{
    public float mouseSens = 100f;

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
        float mouseX = rotate.x * mouseSens * Time.deltaTime;
        float mouseY = rotate.y * mouseSens * Time.deltaTime;

        //control rotation around x axis (Look up and down)
        xLook -= mouseY;

        //we clamp the rotation so we cant Over-rotate (like in real life)
        xLook = Mathf.Clamp(xLook, -90f, 90f);
        //yLook = Mathf.Clamp(yLook, -90f, 90f);

        //control rotation around y axis (Look up and down)
        yLook += mouseX;

        //applying both rotations
        transform.localRotation = Quaternion.Euler(xLook, yLook, 0f);
    }
}

