//using UnityEditor.ShaderGraph;
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

    void Awake()
    {
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
