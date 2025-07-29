using UnityEngine;

public class Enemystate : MonoBehaviour
{
    public static Enemystate Instance { get; set;}

    public float currentHealth;
    public float maxHealth;



     void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
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
        
    }
}
