using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Processors;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour
{
    public AudioSource deadsound;
    public AudioSource injuredsound;
    public GameObject player;
    public float currentHealth;
    public float maxHealth;
    public GameObject healthBar;
    public GameObject gameOverUI;
    public GameObject bullet;
    public float restartDelay = 2f;
    private PlayerMovement playerMovement;
    private MouseMovement mouseMovement;
    private InteractableObject interact;
    public GameObject criticalHealth;
    public DamageHit hit;
    public bool death;

    void Start()
    {
        death = false;
        healthBar.SetActive(true);
        currentHealth = maxHealth;
        playerMovement = player.GetComponent<PlayerMovement>();
        mouseMovement = player.GetComponent<MouseMovement>();
        playerMovement.enabled = true;
        mouseMovement.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        interact = player.GetComponent<InteractableObject>();
        hit = Object.FindFirstObjectByType<DamageHit>();

    }

    private void Update()
    {
        if (currentHealth <= 100 && currentHealth > 0)
        {
            criticalHealth.SetActive(true);
            death = true;
        }
        else
        {
            criticalHealth.SetActive(false);
            death = false;
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            takeDamage(10f);
        }
        else if(other.CompareTag("fire"))
        {
            takeDamage(10f);
            StartCoroutine(dot());
        }
    }
    IEnumerator dot()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1f);
            takeDamage(2.5f);
        }
    }

    public void takeDamage(float damageDealt)
    {
        if (hit != null)
        {
            hit.TriggerDamageEffect();
        }

        currentHealth -= damageDealt;

        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth <= 0)
        {
            dead();
        }
        else
        {
            injuredsound.Play();
        }
    }

    void dead()
    {
        deadsound.Play();
        injuredsound.Stop();
        death = true;
        healthBar.SetActive(false);
        gameOverUI.SetActive(true);
        playerMovement.enabled = false;
        mouseMovement.enabled = false;
      
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
      
    }

    public void Quit()
    {
        Application.Quit();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
