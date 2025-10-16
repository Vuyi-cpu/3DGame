using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject emptyIM;
    public GameObject swordIM;
    public GameObject scytheIM;
    public GameObject KatanaTut;
    public GameObject ScytheTut;

    public string ItemName;
    public SelectionManager SelectionManager;
    GameObject Weapon, key, health, stun;
    PlayerControls controls;
    [SerializeField] private ThrowWeapon throwWeapon;
    public RotatorSwing rotatorSwing;
    public DoorRotate doorRotate;
    public DoorRotate doorRotate2;
    public ButtonGotIt katanaButton;
    public ButtonGotIt scytheButton;
    public PlayerMovement PlayerMovement;
    public MouseMovement MouseMovement;
    public PlayerState PlayerState;

    //vuyi edits
    public Transform gunPos;
    public Transform gunPos2;
    public float range = 10f;
   public  GameObject currentWeapon;
    public GameObject currentSword;
   public GameObject currentScythe;


    public bool swordEquipped;
    public bool scytheEquipped;
    public bool isKey = false;
    public bool isHealth;
    public bool isStun;

    private void Awake()
    {
        doorRotate.locked = true;
        doorRotate2.locked = true;
        rotatorSwing.enabled = false;
        throwWeapon.enabled = false;
        controls = new PlayerControls();
        controls.Player.Interact.performed += ctx =>
        {
            if (SelectionManager.playerCanInteract)
            {
                
                if (Weapon != null || isKey)
                {
                    Pickup();
                }
            }
        };

        controls.Player.Drop.performed += ctx =>
            {
                if (currentWeapon != null) Drop();
            };
            
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    public void Start()
    {
        swordIM.SetActive(false);
        scytheIM.SetActive(false);
    }

    private void Update()
    {
        CheckWeapons();
    }

    public void CheckWeapons()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            if (hit.transform.tag == "sword")
            {
                Weapon = hit.transform.gameObject;
            }
            else if(hit.transform.tag == "scythe")
            {
                Weapon = hit.transform.gameObject;
            }
            else if (hit.transform.tag == "key")
            {
                isKey = true;
                key = hit.transform.gameObject;
                Weapon = null;
            }
            else if (hit.transform.tag == "healthPack") 
            {
                isHealth = true;
                health = hit.transform.gameObject;
                Weapon = null;
            }
            else if (hit.transform.tag == "stun")
            {
                isStun = true;
                stun = hit.transform.gameObject;
                Weapon = null;
            }
        }
    }
    private void Pickup()
    {
        if (isKey)
        {
            Destroy(key);
            doorRotate.locked = false;
            doorRotate2.locked = false;
        }

        if (isHealth)
        {
            PlayerState.currentHealth += 20;
            if (PlayerState.currentHealth > 200) PlayerState.currentHealth = 200;
            Destroy(health);
        }
        
        if (isStun)
        {

        }

        if (Weapon == null) return;

        if (Weapon.tag == "sword" && !swordEquipped)
        {
          
            currentSword = Weapon;
            currentWeapon = currentSword; 
            swordEquipped = true;

           
            currentSword.transform.position = gunPos.position;
            currentSword.transform.SetParent(gunPos);
            currentSword.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            currentSword.GetComponent<Rigidbody>().isKinematic = true;

            
            rotatorSwing.enabled = true;
            emptyIM.SetActive(false);
            swordIM.SetActive(true);

          
            if (KatanaTut != null)
            {
                KatanaTut.SetActive(true);
                PlayerMovement.enabled = false;
                MouseMovement.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                katanaButton.katanaActive = true;
            }
            if (scytheEquipped)
            {
                currentSword.SetActive(false);
            }
            else
            {

                currentSword.SetActive(true);
            }
            return;
        }
        else if (Weapon.tag == "scythe" && !scytheEquipped)
        {
           
            currentScythe = Weapon;
            currentWeapon = currentScythe; 
            scytheEquipped = true;

           
            currentScythe.transform.position = gunPos2.position;
            currentScythe.transform.SetParent(gunPos2);
            currentScythe.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            currentScythe.GetComponent<Rigidbody>().isKinematic = true;

           
            throwWeapon.enabled = true;

          
            emptyIM.SetActive(false);
            scytheIM.SetActive(true);

            if (ScytheTut != null)
            {
                ScytheTut.SetActive(true);
                PlayerMovement.enabled = false;
                MouseMovement.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                scytheButton.scytheActive = true;
            }

            if (swordEquipped)
            {
                currentScythe.SetActive(false);
            }
            else
            {
                currentScythe.SetActive(true);
            }
            return;
        }
    }



    public void Drop()
    {
        if (currentWeapon == null || throwWeapon.isThrown || throwWeapon.isReturning) return;

        if (currentWeapon == currentSword)
        {
            swordEquipped = false;
            swordIM.SetActive(false);
            currentSword.SetActive(true);
        }
        else if (currentWeapon == currentScythe)
        {
            scytheEquipped = false;
            scytheIM.SetActive(false);
            currentScythe.SetActive(true);
            throwWeapon.enabled = false;
        }

        rotatorSwing.enabled = false;
        emptyIM.SetActive(true);

        currentWeapon.transform.parent = null;
        Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Camera.main.transform.forward * 2f, ForceMode.Impulse);
        }
        currentWeapon = null;
    }


    public string GetItemName()
    {
        return ItemName;
    }

}

   
