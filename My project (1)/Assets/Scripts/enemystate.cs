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

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Attack.performed += ctx =>
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.distance < 5 && interactableObject.equipped == true && hit.transform.gameObject == enemy)
            {

                currentHealth -= 10;
                if (currentHealth == 0)
                {
                    Destroy(enemy);
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
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && hit.distance < 8)
        {
            if (hit.transform.gameObject == enemy)
            {
                healthBar.SetActive(true);
            }
            else
            {
                healthBar.SetActive(false);
            }
        }
        else
        {
            healthBar.SetActive(false);
        }

    }
}
