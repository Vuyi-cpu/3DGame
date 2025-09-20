using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public GameObject neuronInfo;
    TextMeshProUGUI neuronText;
    public GameObject upgradeKatanaInfo;
    TextMeshProUGUI upgradeKatanaText;
    public GameObject upgradeScytheInfo;
    TextMeshProUGUI upgradeScytheText;
    public GameObject upgradeHealthInfo;
    TextMeshProUGUI upgradeHealthText;

    public float neuronCount;
    ThrowWeapon throwWeapon;
    Enemystate enemystate;
    PlayerState playerState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        neuronText = neuronInfo.GetComponent<TextMeshProUGUI>();
        upgradeKatanaText = upgradeKatanaInfo.GetComponent<TextMeshProUGUI>();
        upgradeScytheText = upgradeScytheInfo.GetComponent<TextMeshProUGUI>();
        upgradeHealthText = upgradeHealthInfo.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void scytheUpgrade ()
    {
        if (neuronCount >= 0)
        {
            throwWeapon.scytheDamage += 5;
            upgradeScytheText.text = "Damage: " + throwWeapon.scytheDamage.ToString() + " -> " + (throwWeapon.scytheDamage + 5f).ToString();
        }
        else
        {
            
        }
    }

    public void katanaUpgrade()
    {
        if (neuronCount >= 0)
        {
            throwWeapon.scytheDamage += 5;
            upgradeKatanaText.text = "Damage: " + enemystate.katanaDamage.ToString() + " -> " + (enemystate.katanaDamage + 5f).ToString();
        }
        else
        {

        }
    }

    public void healthUpgrade()
    {
        if (neuronCount >= 0)
        {
            playerState.maxHealth += 100;
            playerState.currentHealth += 100;
            upgradeHealthText.text = "Health: " + playerState.maxHealth.ToString() + " -> " + (playerState.currentHealth + 100f).ToString();
        }
        else
        {

        }
    }
}
