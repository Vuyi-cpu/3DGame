using TMPro;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Enemystate : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemy;
    public float currentHealth;
    public float maxHealth;
    public float katanaDamage;
    [SerializeField] private Transform player;

    [Header("References")]
    public InteractableObject interactableObject;
    [SerializeField] private PlayerState playerHealth;
    public PlayerMovement PlayerMovement;
    public MouseMovement MouseMovement;
    public Shop shop;
    public GameObject healthBar;
    public GameObject neuronInfo;
    public GameObject shopTut;
    public ButtonGotIt shopButton;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;

    private TextMeshProUGUI neuronText;
    private PlayerControls controls;
    private bool isDead = false;
    GameObject katana;

    public AudioSource dmg;
    public AudioSource killed;

    void Awake()
    {
        neuronText = neuronInfo.GetComponent<TextMeshProUGUI>();
        controls = new PlayerControls();

        controls.Player.Attack.performed += ctx =>
        {
            if (isDead) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (interactableObject.swordEquipped == true)
            {
                StartCoroutine(SwordSwing());
                if (Physics.Raycast(ray, out hit) && hit.distance < 5 && hit.transform.gameObject == enemy)
                {
                    TakeDamage(shop.katanaDamage);
                }
            }
        };
    }

    void Start()
    {
        currentHealth = maxHealth;

        if (agent == null)
            agent = enemy.GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = enemy.GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        if (agent != null && animator != null)
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed);
        }
    }

    IEnumerator SwordSwing()
    {
        
        katana = interactableObject.currentSword;
        katana.GetComponent<Animator>().Play("swordSwing", 0, 0f);
        yield return new WaitForSeconds(0.73f);
        katana.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        katana.transform.position = interactableObject.gunPos.position;
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            killed.Play();
            // Stop agent and trigger death animation
            if (agent != null) agent.isStopped = true;
            if (animator != null)
            {
                animator.SetBool("IsDead", true);
            }

            // Disable collider
            Collider col = enemy.GetComponent<Collider>();
            if (col) col.enabled = false;

            // Rewards / UI
            if (shopTut != null)
            {
                shopTut.SetActive(true);
                PlayerMovement.enabled = false;
                MouseMovement.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                shopButton.shopActive = true;
            }

            playerHealth.currentHealth = Mathf.Min(playerHealth.currentHealth + 50, playerHealth.maxHealth);
            shop.neuronCount += 80f;
            neuronText.text = shop.neuronCount.ToString();

            // Destroy after delay
            Destroy(enemy, 3f);
        }
        else
        {
            dmg.Play();
        }
    }

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();
}
