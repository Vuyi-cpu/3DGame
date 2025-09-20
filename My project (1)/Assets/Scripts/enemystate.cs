using TMPro;
using UnityEngine;

public class Enemystate : MonoBehaviour
{
    public GameObject enemy;
    public float currentHealth;
    public float maxHealth;
    public float katanaDamage;
    [SerializeField] private Transform player;
    public InteractableObject interactableObject;
    PlayerControls controls;
    public GameObject healthBar;
    [SerializeField] PlayerState playerHealth;
    public GameObject neuronInfo;
    TextMeshProUGUI neuronText;
    public Shop shop;
    public RotatorSwing rotatorSwing;

    void Awake()
    {
        neuronText = neuronInfo.GetComponent<TextMeshProUGUI>();
        controls = new PlayerControls();
        controls.Player.Attack.performed += ctx =>
        {
            //if (!rotatorSwing.isSwinging && interactableObject.swordEquipped == true) rotatorSwing.StartSwing();
        };
    }

    void Start()
    {
        currentHealth = maxHealth;
    }
    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && hit.distance < 5 && interactableObject.swordEquipped == true && hit.transform.gameObject == enemy)
        {
            currentHealth -= katanaDamage;
            if (currentHealth <= 0)
            {
                Destroy(enemy);
                playerHealth.currentHealth += 100;
                if (playerHealth.currentHealth >= playerHealth.maxHealth) playerHealth.currentHealth = playerHealth.maxHealth;
                shop.neuronCount += 50f;
                neuronText.text = shop.neuronCount.ToString();
            }
        }
    }
}
