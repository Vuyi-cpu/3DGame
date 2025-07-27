using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{

    public float mouseSens = 100f;

    float xLook = 0f;
    float yLook = 0f;

    void Start()
    {
        //Locking the cursor to the middle of the screen and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

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
