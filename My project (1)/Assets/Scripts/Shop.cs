using TMPro;
using UnityEngine;

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

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        
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
            katanaDamage += 5;
            upgradeKatanaText.text = "Damage: " + katanaDamage.ToString() + " -> " + (katanaDamage + 5f).ToString();
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
