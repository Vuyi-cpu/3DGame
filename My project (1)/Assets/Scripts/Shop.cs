using TMPro;
using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour
{
    //public GameObject neuronInfo;
    public TextMeshProUGUI neuronText;
    //public GameObject upgradeKatanaInfo;
    public TextMeshProUGUI upgradeKatanaText;
    //public GameObject upgradeScytheInfo;
    public TextMeshProUGUI upgradeScytheText;
    //public GameObject upgradeHealthInfo;
    public TextMeshProUGUI upgradeHealthText;

    public float neuronCount;
    public float katanaDamage;
    public ThrowWeapon throwWeapon;
    public Enemystate enemystate;
    public PlayerState playerState;

    public float pulseScale = 1.5f;
    public float pulseDuration = 0.2f;
    public TextMeshProUGUI insufficientFundsText;
    private bool isPulsing = false;
    private Vector3 originalScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (insufficientFundsText != null)
        {
            originalScale = insufficientFundsText.transform.localScale;
        }
    }
    // Update is called once per frame
    void Update()
    {
        neuronText.text = neuronCount.ToString();
    }

    public void scytheUpgrade ()
    {
        if (neuronCount >= 100)
        {
            neuronCount -= 100;
            insufficientFundsText.gameObject.SetActive(false);
            throwWeapon.scytheDamage += 5;
            upgradeScytheText.text = "Damage: " + throwWeapon.scytheDamage.ToString() + " -> " + (throwWeapon.scytheDamage + 5f).ToString();
        }
        else
        {
            StartCoroutine(PulseText(insufficientFundsText));
           
        }
    }

    public void katanaUpgrade()
    {
        if (neuronCount >= 100)
        {
            neuronCount -= 100;
            insufficientFundsText.gameObject.SetActive(false);
            katanaDamage += 5;
            upgradeKatanaText.text = "Damage: " + katanaDamage.ToString() + " -> " + (katanaDamage + 5f).ToString();
        }
        else
        {
            StartCoroutine(PulseText(insufficientFundsText));
        }
    }

    public void healthUpgrade()
    {
        if (neuronCount >= 100)
        {
            neuronCount -= 100;
            insufficientFundsText.gameObject.SetActive(false);
            playerState.maxHealth += 50;
            playerState.currentHealth += 50;
            upgradeHealthText.text = "Health: " + playerState.maxHealth.ToString() + " -> " + (playerState.currentHealth + 50f).ToString();
        }
        else
        {
            StartCoroutine(PulseText(insufficientFundsText));
        }
    }

    private IEnumerator PulseText(TextMeshProUGUI text)
    {
        if (isPulsing)
        {
            text.transform.localScale = originalScale; // reset if mid-pulse
            isPulsing = false;
        }

        isPulsing = true;
        text.gameObject.SetActive(true);

        Vector3 targetScale = originalScale * pulseScale;
        float timer = 0f;

        // Scale up
        while (timer < pulseDuration)
        {
            text.transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / pulseDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        text.transform.localScale = targetScale;

        // Scale back down
        timer = 0f;
        while (timer < pulseDuration)
        {
            text.transform.localScale = Vector3.Lerp(targetScale, originalScale, timer / pulseDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        text.transform.localScale = originalScale;

        isPulsing = false;
    }


}
