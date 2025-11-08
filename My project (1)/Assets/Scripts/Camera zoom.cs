using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    private PlayerControls controls;

    [SerializeField] private float zoomFov = 30f;
    [SerializeField] private float normalFov = 60f;
    [SerializeField] private float zoomSpeed = 5f;

    private Camera cam;
    private bool isZooming = false;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Zoom.performed += ctx => isZooming = true;
        controls.Player.Zoom.canceled += ctx => isZooming = false;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        float targetFov = isZooming ? zoomFov : normalFov;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, Time.deltaTime * zoomSpeed);
    }
}
