using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.tvOS;

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
    public GameObject pause;
    public bool pauseactive;
    public GameObject pausefirst;
    PlayerState state;



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
    public AudioSource wakemusic;

    private void Awake()
    {
        MouseMovement = GetComponentInParent<MouseMovement>();
        active = false;
        pauseactive = false;
        controls = new PlayerControls();
        state = Object.FindFirstObjectByType<PlayerState>();
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
        //controls.Player.Jump.performed += ctx => jumpPressed = true;
        controls.Player.Dash.performed += ctx => StartCoroutine(Dash());
        controls.Player.Shop.performed += ctx =>
        {
            if (!active && !pauseactive && state.death == false)
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

        controls.Player.Pause.performed += ctx =>
        {
            MouseMovement.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pause.SetActive(true);
            pauseactive = true;
            Time.timeScale = 0f;

            EventSystem.current.SetSelectedGameObject(pausefirst);

        };
    }

    private void Update()
    {
        if (active || pauseactive)
        {
            if (wakemusic.isPlaying)
                wakemusic.Stop();
            return;
        }

        if (!wakemusic.isPlaying)
            wakemusic.Play();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 moveDir = transform.right * move.x + transform.forward * move.y;
        controller.Move(moveDir * speed * Time.deltaTime);

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
