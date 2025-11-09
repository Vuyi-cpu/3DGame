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
    public int requiredKills;
    public Shop shop;
    public GameObject requiredKey;
    public InteractableObject interactable;


    public GameObject interaction_Info_UI;
    private TextMeshProUGUI interaction_text;

    public Transform door1;
    public Transform door2;

    PlayerControls controls;
    Enemystate enemystate;

    public AudioSource fightSong;
    public AudioSource wakeSong;
    private bool fightPlayed = false;

    public GameObject pause;

    private AudioSource lastPlayedSong;

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
        if (wakeSong != null)
        {
            lastPlayedSong = wakeSong;
            wakeSong.Play();
        }
    }

    void Update()
    {
        if (pause != null && pause.activeSelf)
        {
            if (fightSong != null && fightSong.isPlaying)
            {
                fightSong.Pause();
                lastPlayedSong = fightSong;
            }
            if (wakeSong != null && wakeSong.isPlaying)
            {
                wakeSong.Pause();
                lastPlayedSong = wakeSong;
            }
            if (dooropenSound.isPlaying)
                dooropenSound.Stop();
            return;
        }
        else
        {
            if (lastPlayedSong != null && !lastPlayedSong.isPlaying)
            {
                lastPlayedSong.UnPause();
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            if ((locked && (hit.transform == door1 || hit.transform == door2)))
            {

                interaction_text.text = "Locked.";
                interaction_Info_UI.SetActive(true);
                return;
            }
            else if (hit.transform == door1 || hit.transform == door2)
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
        if (locked)
        {
            if (interactable == null || interactable.key == null || requiredKey == null || interactable.key.name != requiredKey.name)
            {
                interaction_text.text = "Locked.";
                yield break;
            }
            else
            {
                locked = false;
              
            }
        }

        if ((shop.EnemiesKilled / 50) < requiredKills)
        {
            interaction_text.text = "Locked.";
            yield break;
        }

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

            if (!fightPlayed && fightSong != null)
            {
                if (wakeSong != null && wakeSong.isPlaying)
                    wakeSong.Stop();
                fightSong.Play();
                lastPlayedSong = fightSong;
                fightPlayed = true;
            }
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



