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
    public GameObject BossKey;
    public ButtonGotIt shopButton;

    [Header("Animation")]
    private Animator animator;
    private NavMeshAgent agent;

    private TextMeshProUGUI neuronText;
    private PlayerControls controls;
    private bool isDead = false;
    private bool swinging;
    GameObject katana;
    Level2 level2;
    EnemyAI enemyAI;
   


    public AudioSource dmg;
    public AudioSource killed;

    public ParticleSystem scrapeParticles;
    public ParticleSystem stunParticles;

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        level2 = FindFirstObjectByType<Level2>();
        stunParticles.Stop();
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
                if (Physics.Raycast(ray, out hit) && swinging && hit.transform.gameObject == enemy && hit.distance <= 3)
                {
                    scrapeParticles.transform.position = hit.point;
                    scrapeParticles.transform.rotation = Quaternion.LookRotation(hit.normal);
                    scrapeParticles.Play();
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
        swinging = true;
        katana = interactableObject.currentSword;
        katana.GetComponent<Animator>().Play("swordSwing", 0, 0f);
        yield return new WaitForSeconds(0.73f);
        katana.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        katana.transform.position = interactableObject.gunPos.position;
        swinging = false;
    }

    private void TakeDamage(float damage)
    {
        if (enemy.CompareTag("Shooter") || enemy.CompareTag("Melee") || enemy.CompareTag("Daisuke") || enemy.CompareTag("Boss"))
            {
                if (enemy != null)
                {
                    currentHealth -= damage;
                    if (currentHealth <= 0)
                    {
                    
                        if (!enemy.CompareTag("Boss")) killed.Play();
                        Destroy(enemy);
                        if (enemy.CompareTag("Boss"))
                        {
                            level2.BossDead = true;
                        }
                        if (enemy.CompareTag("Shooter") || enemy.CompareTag("Melee"))
                        {
                            playerHealth.currentHealth += 50;
                            if (playerHealth.currentHealth >= playerHealth.maxHealth) playerHealth.currentHealth = playerHealth.maxHealth;
                            shop.neuronCount += 50f;
                            shop.EnemiesKilled += 50f;
                            
                        }
                        else if (enemy.CompareTag("Daisuke"))
                        {
                            BossKey.SetActive(true);
                            playerHealth.currentHealth += 100f;
                            if (playerHealth.currentHealth >= playerHealth.maxHealth) playerHealth.currentHealth = playerHealth.maxHealth;
                            shop.neuronCount += 200f;
                        }
                        neuronText.text = shop.neuronCount.ToString();
                    }
                    else
                    {
                    if (!enemy.CompareTag("Boss")) dmg.Play();
                    }
                }
            }  
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("stun"))
        {
            enemyAI = GetComponentInParent<EnemyAI>();
            enemyAI.enabled = false;
            Debug.Log("AI disabled on: " + enemy.name);
            StartCoroutine(stunEnemy());
        }
    }

    public IEnumerator stunEnemy()
    {
        stunParticles.transform.position = enemy.transform.position;
        stunParticles.Play();
        yield return new WaitForSeconds(3f);
        if (enemy != null) enemyAI.enabled = true;
        Debug.Log("AI renabled");
        stunParticles.Stop();
    }

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();
}
