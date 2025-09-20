using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    PlayerControls controls;
    Vector2 move;
    public bool jumpPressed, active;
    public GameObject shop;
    MouseMovement MouseMovement;
    public ThrowWeapon throwWeapon;
    public InteractableObject interactableObject;

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;


    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    //Dashing
    public float dashFov;
    public PlayerCam cam;
    public float DashSpd;
    public float DashTime;
    public float decay;
    float count;

    private void Awake()
    {
        MouseMovement = GetComponentInParent<MouseMovement>();
        active = false;
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
        controls.Player.Jump.performed += ctx => jumpPressed = true;
        controls.Player.Dash.performed += ctx => StartCoroutine(Dash());
        controls.Player.Shop.performed += ctx => {
                if (!active)
                {
                    MouseMovement.enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    shop.SetActive(true);
                    active = true;
                }
                else if (active)
                {
                    shop.SetActive(false);
                    active = false;
                    MouseMovement.enabled = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            
        };
    }

    // Update is called once per frame
    private void Update()
    {
<<<<<<< Updated upstream
=======
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (!active)
            {
                MouseMovement.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                shop.SetActive(true);
                active = true;
            }
            else if (active)
            {
                shop.SetActive(false);
                active = false;
                MouseMovement.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
>>>>>>> Stashed changes
        //checking if we hit the ground to reset our falling velocity, otherwise we will fall faster the next time
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 moveDir = transform.right * move.x + transform.forward * move.y;
        controller.Move(moveDir * speed * Time.deltaTime);

        //check if the player is on the ground so he can jump
        if (jumpPressed && isGrounded)
        {
            //the equation for jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpPressed = false;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        Vector3 dashDir = (transform.right * move.x + transform.forward * move.y).normalized;
        cam.DoFov(dashFov, 0.1f);

        float dashSpeed = DashSpd;

        float elapsed = 0f;
        while (elapsed < DashTime)
        {
            controller.Move(dashDir * dashSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cam.DoFov(60f, 0.2f);
        //after dash ends, keep momentum and smoothly reduce it
        float momentum = dashSpeed;
        while (momentum > 0.1f)
        {
            momentum = Mathf.Lerp(momentum, 0f, Time.deltaTime * decay); 
            controller.Move(dashDir * momentum * Time.deltaTime);
            yield return null;
        }
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}