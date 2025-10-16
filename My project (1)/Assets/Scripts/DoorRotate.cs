using System.Collections;
using TMPro;
using UnityEngine;

public class DoorRotate : MonoBehaviour
{
    public AudioSource dooropenSound;
    public AudioSource doorclosSound;
    public float open = 90f;
    public float speed = 2f;
    public int mult;
    bool isopen = false;
    public bool locked;
    private Quaternion rotationshut;
    private Quaternion rotationopen;
    private Coroutine coroutine;

    public GameObject interaction_Info_UI;
    private TextMeshProUGUI interaction_text;

    public Transform door1;
    public Transform door2;

    PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Interact.performed += ctx =>
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 5f))
            {
                if (hit.transform == door1 || hit.transform == door2)
                {
                    if (coroutine != null) StopCoroutine(coroutine);
                    coroutine = StartCoroutine(MoveDoor());
                }
            }
        };
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
        rotationshut = transform.rotation;
        rotationopen = Quaternion.Euler(transform.eulerAngles + new Vector3(0, -open + mult * 35f, 0));
        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            if (locked && (hit.transform == door1 || hit.transform == door2))
            {
                interaction_text.text = "Locked.";
                interaction_Info_UI.SetActive(true);
                return;
            }
            else if(hit.transform == door1 || hit.transform == door2)
            {
                interaction_text.text = "[E] to interact.";
                interaction_Info_UI.SetActive(true);
                return;
            }
        }
        else
        {
             interaction_Info_UI.SetActive(false);
        }


    }

    private IEnumerator MoveDoor()
    {
        if (!locked)
        {
          
            if (isopen)
            {
                doorclosSound.Stop();
                dooropenSound.Stop();
                doorclosSound.Play();  
            }
            else
            {
                dooropenSound.Stop();
                doorclosSound.Stop();
                dooropenSound.Play();  
            }

            Quaternion endRotate = isopen ? rotationshut : rotationopen;
            isopen = !isopen;

            while (Quaternion.Angle(transform.rotation, endRotate) > 0.01f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, endRotate, Time.deltaTime * speed);
                yield return null;
            }

            transform.rotation = endRotate;
        }
    }

}