using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealthbar : MonoBehaviour
{
    private Slider slide;
    public TextMeshProUGUI healthText;
    public GameObject state;
    private float currentHealth, maxHealth;
    void Awake()
    {
        slide = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = state.GetComponent< PlayerState > ().currentHealth;
        maxHealth = state.GetComponent< PlayerState > ().maxHealth;
        float fillValue = currentHealth / maxHealth;
        slide.value = fillValue;
        healthText.text = currentHealth + "/" + maxHealth;

    }
}
