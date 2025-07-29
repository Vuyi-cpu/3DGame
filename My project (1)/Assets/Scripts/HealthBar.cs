using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{

    private Slider slide;
    public TextMeshProUGUI healthText;
    public GameObject enemyState;
    private float currentHealth, maxHealth;
    void Awake()
    {
        slide = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = enemyState.GetComponent<Enemystate>().currentHealth;
        maxHealth = enemyState.GetComponent<Enemystate>().maxHealth;
        float fillValue = currentHealth / maxHealth;
        slide.value = fillValue;
        healthText.text = currentHealth + "/" + maxHealth;

    }
}
