//using UnityEditor.ShaderGraph;
using TMPro;
using UnityEngine;

public class Enemystate : MonoBehaviour
{
    public GameObject enemy;
    public float currentHealth;
    public float maxHealth;
    [SerializeField] private Transform player;
    public InteractableObject interactableObject;
    PlayerControls controls;
    public GameObject healthBar;
    [SerializeField] PlayerState playerHealth;
    public GameObject neuronInfo;
    TextMeshProUGUI neuronText;
    private float neuronCount;

    void Awake()
    {
        neuronText = neuronInfo.GetComponent<TextMeshProUGUI>();
        controls = new PlayerControls();
        controls.Player.Attack.performed += ctx =>
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.distance < 5 && interactableObject.swordEquipped == true && hit.transform.gameObject == enemy)
            {
                currentHealth -= 10;
                if (currentHealth == 0)
                {
                    Destroy(enemy);
                    playerHealth.currentHealth += 100;
                    if (playerHealth.currentHealth >= playerHealth.maxHealth) playerHealth.currentHealth = playerHealth.maxHealth;
                    neuronCount += 50f;
                    neuronText.text = neuronCount.ToString();
                }
            }

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
    void Update()
    {
        

    }
}
