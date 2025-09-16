using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public GameObject player;
    public float currentHealth;
    public float maxHealth;
    public GameObject healthBar;
    public GameObject bullet;

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
      
        if (other.CompareTag("Bullet"))
        {
            currentHealth -= 10;
         
            currentHealth = Mathf.Max(currentHealth, 0);


            if (currentHealth == 0)
            {
                Debug.Log("Player is dead!");
            }
        }
    }
}
