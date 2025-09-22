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
    public PlayerMovement PlayerMovement;
    public MouseMovement MouseMovement;
    public GameObject shopTut;
    public ButtonGotIt shopButton;

    void Awake()
    {
        neuronText = neuronInfo.GetComponent<TextMeshProUGUI>();
        controls = new PlayerControls();
        controls.Player.Attack.performed += ctx =>
        {
            //if (interactableObject.swordEquipped == true && !rotatorSwing.isSwinging) rotatorSwing.StartSwing();
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hit) && hit.distance < 5 && interactableObject.swordEquipped == true && hit.transform.gameObject == enemy)
            {

                currentHealth -= shop.katanaDamage;
                if (currentHealth <= 0)
                {
                    if (shopTut != null)
                    {
                        shopTut.SetActive(true);
                        PlayerMovement.enabled = false;
                        MouseMovement.enabled = false;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        shopButton.shopActive = true;
                    }
                    Destroy(enemy);
                    playerHealth.currentHealth += 50;
                    if (playerHealth.currentHealth >= playerHealth.maxHealth) playerHealth.currentHealth = playerHealth.maxHealth;
                    shop.neuronCount += 50f;
                    neuronText.text = shop.neuronCount.ToString();
                }
            }
        };
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
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

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
