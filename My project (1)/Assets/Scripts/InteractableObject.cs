using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    public GameObject emptyIM;
    public GameObject swordIM;
    public GameObject scytheIM;
    public GameObject KatanaTut;
    public GameObject ScytheTut;

    public string ItemName;
    public SelectionManager SelectionManager;
    GameObject Weapon, health;
    public GameObject key;
    PlayerControls controls;
    [SerializeField] public ThrowWeapon throwWeapon;
    public DoorRotate doorRotate;
    public DoorRotate doorRotate2;
    public ButtonGotIt katanaButton;
    public ButtonGotIt scytheButton;
    public PlayerMovement PlayerMovement;
    public MouseMovement MouseMovement;
    public PlayerState PlayerState;
    StunThrow stunThrow;
    public StunThrow stunThrow1;
    //public StunThrow stunThrow2;

    //vuyi edits
    public Transform gunPos;
    public Transform gunPos2;
    public float range = 10f;
    public GameObject currentWeapon;
    public GameObject currentSword;
    public GameObject currentScythe;
    public GameObject currentStun;
    public Rotator stunRotate;

    public GameObject activeWeapon; 

    public bool swordEquipped;
    public bool scytheEquipped;
    public bool stunEquipped;
    public bool isKey = false;
    public bool isHealth;
    public bool isStun;
    public static List<InteractableObject> AllInteractables = new List<InteractableObject>();
    public bool isStunned;

    public AudioSource drop;
    public AudioSource pickup;

    private void Awake()
    {
        doorRotate.locked = true;
        doorRotate2.locked = true;
        throwWeapon.enabled = false;
        stunThrow1.enabled = false;
        //stunThrow2.enabled = false;
        stunRotate.enabled = false;
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

        controls.Player.scythe.performed += ctx =>
        {
            if (scytheEquipped) 
            {
                if (swordEquipped) currentSword.SetActive(false);
                if (stunEquipped) currentStun.SetActive(false);
                currentScythe.SetActive(true);
                activeWeapon = currentScythe; 
            }
        };

        controls.Player.Katana.performed += ctx =>
        {
            if (swordEquipped && !(throwWeapon.isThrown || throwWeapon.isReturning)) 
            {
                if (scytheEquipped) currentScythe.SetActive(false);
                if (stunEquipped) currentStun.SetActive(false);
                currentSword.SetActive(true);
                activeWeapon = currentSword; 
            }
        };

        controls.Player.Stun.performed += ctx =>
        {
            if (stunEquipped && !(throwWeapon.isThrown || throwWeapon.isReturning))
            {
                if (scytheEquipped) currentScythe.SetActive(false);
                if (swordEquipped) currentSword.SetActive(false);
                currentStun.SetActive(true);
                activeWeapon = currentStun;
            }
        };

    }


    void OnEnable()
    {
        AllInteractables.Add(this);
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
        AllInteractables.Remove(this);
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
            else if (hit.transform.tag == "scythe")
            {
                Weapon = hit.transform.gameObject;
            }
            else if (hit.transform.tag == "stun")
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
            
        }
    }

    private void Pickup()
    {
        if (isKey)
        {
            pickup.Play();
            key.SetActive(false);
            doorRotate.locked = false;
            doorRotate2.locked = false;
        }

        if (isHealth)
        {
            pickup.Play();
            PlayerState.currentHealth += 70;
            if (PlayerState.currentHealth > 200) PlayerState.currentHealth = 200;
            Destroy(health);
        }

        if (Weapon == null) return;

        if (Weapon.tag == "stun" && !stunEquipped)
        {
            stunThrow = Weapon.GetComponent<StunThrow>();
            currentStun = Weapon;
            stunEquipped = true;

            currentStun.transform.position = gunPos.position;
            currentStun.transform.SetParent(gunPos);
            currentStun.transform.localEulerAngles = new Vector3(0f, -10f, 0f);
            currentStun.GetComponent<Rigidbody>().isKinematic = true;
            stunThrow.enabled = true;

            if (scytheEquipped) currentScythe.SetActive(false);
            if (swordEquipped) currentSword.SetActive(false);
        }

        if (Weapon.tag == "sword" && !swordEquipped)
        {
            currentSword = Weapon;
            currentWeapon = currentSword;
            swordEquipped = true;

            currentSword.transform.position = gunPos.position;
            currentSword.transform.SetParent(gunPos);
            currentSword.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            currentSword.GetComponent<Rigidbody>().isKinematic = true;

            emptyIM.SetActive(false);
            swordIM.SetActive(true);

            if (KatanaTut != null)
            {
                pickup.Play();
                KatanaTut.SetActive(true);
                PlayerMovement.enabled = false;
                MouseMovement.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                katanaButton.katanaActive = true;
            }
            
            if (scytheEquipped) currentScythe.SetActive(false);
            if (stunEquipped) currentStun.SetActive(false);

            currentSword.SetActive(true);
            activeWeapon = currentSword;
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
                pickup.Play();
                ScytheTut.SetActive(true);
                PlayerMovement.enabled = false;
                MouseMovement.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                scytheButton.scytheActive = true;
            }

            currentScythe.SetActive(true);
            activeWeapon = currentScythe;
            key.SetActive(true);
            return;
        }
    }

    //public void Drop()
    //{
    //    if (activeWeapon == null || throwWeapon.isThrown || throwWeapon.isReturning) return;

    //    drop.Play();
    //    if (activeWeapon == currentSword)
    //    {
    //        swordEquipped = false;
    //        swordIM.SetActive(false);
    //        rotatorSwing.enabled = false;
    //    }
    //    else if (activeWeapon == currentScythe)
    //    {
    //        scytheEquipped = false;
    //        scytheIM.SetActive(false);
    //        throwWeapon.enabled = false;
    //    }

    //    emptyIM.SetActive(true);

      
    //    activeWeapon.transform.parent = null;
    //    Rigidbody rb = activeWeapon.GetComponent<Rigidbody>();
    //    if (rb != null)
    //    {
    //        rb.isKinematic = false;
    //        rb.useGravity = true;
    //        rb.AddForce(Camera.main.transform.forward * 2f, ForceMode.Impulse);
    //    }

    //    activeWeapon = null; 

        
    //    if (swordEquipped)
    //    {
    //        currentSword.SetActive(true);
    //        activeWeapon = currentSword;
    //        swordIM.SetActive(true);
    //        rotatorSwing.enabled = true;
    //        emptyIM.SetActive(false);
    //    }
    //    else if (scytheEquipped)
    //    {
    //        currentScythe.SetActive(true);
    //        activeWeapon = currentScythe;
    //        scytheIM.SetActive(true);
    //        throwWeapon.enabled = true;
    //        emptyIM.SetActive(false);
    //    }
    //}


    public string GetItemName()
    {
        return ItemName;
    }
}

