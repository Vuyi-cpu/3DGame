using UnityEngine;

public class Enemystate : MonoBehaviour
{
    public static Enemystate Instance { get; set; }
    public GameObject enemy;

    public float currentHealth;
    public float maxHealth;



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
        if (Input.GetKeyDown(KeyCode.B))
        {
            currentHealth -= 10;
            if (currentHealth == 0)
            {
                enemy.SetActive(false);
            }
        }
    }
}
