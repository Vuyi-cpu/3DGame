using UnityEngine;

public class Enemystate : MonoBehaviour
{
    public static Enemystate Instance { get; set; }
    public GameObject enemy;
    public float currentHealth;
    public float maxHealth;
    [SerializeField] private Transform player;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(enemy);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && Input.GetKeyDown(KeyCode.Mouse0) && hit.distance < 5)
        {
            currentHealth -= 10;
            if (currentHealth == 0)
            {
                enemy.SetActive(false);
            }
        }
    }
}
