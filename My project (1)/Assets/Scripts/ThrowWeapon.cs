using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UIElements;

public class ThrowWeapon : MonoBehaviour
{
    [SerializeField] GameObject scythe; //Game object reference to scythe. Used to move scythe
    [SerializeField] Rigidbody scytheRb;
    [SerializeField] Transform scytheLocation; //Original scythe location
    [SerializeField] float scytheDistance; //How far can we throw this thing
    [SerializeField] float throwSpeed; //Throw speed of this thing
    [SerializeField] LayerMask layerMask; //Layer mask for raycast check. Looking for environment layer
    [SerializeField] private Transform player; //Player transform
    public GameObject neuronInfo;
    TextMeshProUGUI neuronText;

    public bool isThrown; //Bool that gets set when thrown
    public bool isReturning; //Bool that get set after hitting the middle point

    [SerializeField] Vector3 throwPosition; //This is where the scythe is traveling to.
    [SerializeField] Rotator rotator; //Rotator on scythe object. Gets turned on when thrown. And off when not.
    [SerializeField] PlayerState playerHealth;

    public float scytheDamage; //How much scytheDamage does this object do?
    private float currentReturnSpeed;
    public Shop shop;
    PlayerControls controls;
    public GameObject BossKey;
    public PlayerMovement PlayerMovement;
    public MouseMovement MouseMovement;
    public GameObject shopTut;
    public ButtonGotIt shopButton;

    public ParticleSystem scrapeParticles;
    public ParticleSystem scrapeParticles2;
    public ParticleSystem distortParticles;
    public TrailRenderer trail;
    Level2 level2;

    public AudioSource dmg;
    public AudioSource dmg2;
    public AudioSource killed;
    public AudioSource killed2;

    private void Awake()
    {
        trail.emitting = false;
        distortParticles.Stop();
        neuronText = neuronInfo.GetComponent<TextMeshProUGUI>();
        rotator.enabled = false;
        controls = new PlayerControls();
        if (!PlayerMovement.active)
        {
            controls.Player.Attack.performed += ctx =>
            {
                //If isThrown or isReturning is true, go away, else check distance
                if (isThrown || isReturning) return;
                CheckDistance();
            };
        }
        level2 = FindFirstObjectByType<Level2>();
    }

    void Update()
    {
        
            //If isThrown is true
            if (isThrown)
            {
                trail.emitting = true;
                distortParticles.Play();
                
                //Set new position to move towards and apply to scythe transform.
                Vector3 newPos = Vector3.MoveTowards(scythe.transform.position, throwPosition, throwSpeed * Time.deltaTime);
                scythe.transform.position = newPos;

                //If the scythe's position is equal to the throw position
                if (scythe.transform.position == throwPosition)
                {
                    //Reset both throw variables
                    isThrown = false;
                    isReturning = true;
                }
            }

            //Is isReturning
            if (isReturning)
            {
                trail.emitting = true;
                distortParticles.Play();
                currentReturnSpeed = throwSpeed+2;
                //currentReturnSpeed += (float)(throwSpeed + 0.5 * Time.deltaTime);
                //Set the new position back to the scythe's original position
                Vector3 newPos = Vector3.MoveTowards(scythe.transform.position, scytheLocation.position, currentReturnSpeed * Time.deltaTime);
                scythe.transform.position = newPos;

                //If scythe's position is equal to original scythe location
                if (scythe.transform.position == scytheLocation.position)
                {
                    //Set isReturning to false, turn off rotator, set parent and rotation
                    distortParticles.Stop();
                    trail.emitting = false;
                    scytheRb.isKinematic = true;
                    isReturning = false;
                    rotator.enabled = false;
                    scythe.transform.parent = scytheLocation;
                    scythe.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                }
            }
    }

    //Called from update, Checks distance then throws scythe
    void CheckDistance()
    {
        {
            scytheLocation.localEulerAngles = new Vector3(0f, 0f, 0f);
            throwPosition = player.transform.position + scytheLocation.forward * scytheDistance;
            //Set parent to null, enable rotator, and set isthrown to true so it starts to travel
            scythe.transform.parent = null;
            rotator.enabled = true;
            isThrown = true;
            scytheRb.isKinematic = false;
            scytheRb.useGravity = false;
            scytheLocation.localEulerAngles = new Vector3(63f, -195.3f, -267.07f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        //Damage enemy if hit when returning or throwing
        if (isReturning || isThrown)
        {
            isThrown = false;
            isReturning = true;

            if (collision.gameObject.CompareTag("Shooter") || collision.gameObject.CompareTag("Melee") || collision.gameObject.CompareTag("Daisuke")|| collision.gameObject.CompareTag("Boss"))
            {
                ContactPoint contact = collision.contacts[0];
                if (collision.gameObject.CompareTag("Shooter") || collision.gameObject.CompareTag("Melee") )
                {
                    scrapeParticles.transform.position = contact.point;
                    scrapeParticles.transform.rotation = Quaternion.LookRotation(contact.normal);
                    scrapeParticles.Play();
                }
                if (collision.gameObject.CompareTag("Daisuke"))
                {
                    scrapeParticles2.transform.position = contact.point;
                    scrapeParticles2.transform.rotation = Quaternion.LookRotation(contact.normal);
                    scrapeParticles2.Play();
                }

                Enemystate enemy = collision.gameObject.GetComponentInParent<Enemystate>();
                if (enemy != null)
                {
                    enemy.currentHealth -= scytheDamage;
                    if (enemy.currentHealth <= 0)
                    {
                        if (enemy.CompareTag("Daisuke"))
                        {

                            BossKey.SetActive(true);
                        }
                        if (enemy.CompareTag("Boss"))
                        {
                            level2.BossDead = true;
                        }
                        Destroy(enemy.enemy);
                        if (collision.gameObject.CompareTag("Shooter") || collision.gameObject.CompareTag("Melee"))
                        {
                            killed.Play();
                            if (shopTut != null)
                            {
                                shopTut.SetActive(true);
                                PlayerMovement.enabled = false;
                                MouseMovement.enabled = false;
                                UnityEngine.Cursor.lockState = CursorLockMode.None;
                                UnityEngine.Cursor.visible = true;
                                shopButton.shopActive = true;
                            }
                            
                            playerHealth.currentHealth += 50;
                            if (playerHealth.currentHealth >= playerHealth.maxHealth) playerHealth.currentHealth = playerHealth.maxHealth;
                            shop.neuronCount += 50f;
                            shop.EnemiesKilled += 50f;
                            
                        }
                        else if (collision.gameObject.CompareTag("Daisuke"))
                        {
                            killed2.Play();
                            playerHealth.currentHealth += 100f;
                            if (playerHealth.currentHealth >= playerHealth.maxHealth) playerHealth.currentHealth = playerHealth.maxHealth;
                            shop.neuronCount += 200f;
                        }
                        neuronText.text = shop.neuronCount.ToString();
                    }
                    else
                    {
                        if (!collision.gameObject.CompareTag("Daisuke")) dmg.Play();
                        else { dmg2.Play(); }
                    }
                }
            }  
        }
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

}


