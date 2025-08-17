using UnityEditor.ShaderGraph;
using UnityEngine;

public class Enemystate : MonoBehaviour
{
    public GameObject enemy;
    public float currentHealth;
    public float maxHealth;
    [SerializeField] private Transform player;
    public InteractableObject interactableObject;
    PlayerControls controls;


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



    // Update is called once per frame
    void Update()
    {
 
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
