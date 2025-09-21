using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject emptyIM;
    public GameObject swordIM;
    public GameObject scytheIM;
    public string ItemName;
    GameObject currentSword;
    GameObject currentScythe;
    public SelectionManager SelectionManager;
    GameObject Weapon, key;
    PlayerControls controls;
    [SerializeField] private ThrowWeapon throwWeapon;
    public RotatorSwing rotatorSwing;
    public DoorRotate doorRotate;
    public DoorRotate doorRotate2;

    //vuyi edits
    public Transform gunPos;
    public Transform gunPos2;
    public float range = 10f;
    GameObject currentWeapon;
    

    public bool swordEquipped;
    public bool scytheEquipped;
    public bool isKey = false;

    private void Awake()
    {
        rotatorSwing.enabled = false;
        throwWeapon.enabled = false;
        controls = new PlayerControls();
        controls.Player.Interact.performed += ctx =>
        {
            if ( SelectionManager.playerCanInteract == true)
            {
                if (currentWeapon == null || isKey) Pickup();
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

        if (Weapon == null) return;

        if (Weapon.tag == "sword" && !swordEquipped)
        {
            currentWeapon = Weapon;
            rotatorSwing.enabled = true;
            swordEquipped = true;
            emptyIM.SetActive(false);
            swordIM.SetActive(true);
            currentWeapon.transform.position = gunPos.position;
            currentWeapon.transform.parent = gunPos;
            currentWeapon.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
            return;
        }
        else if (Weapon.tag == "scythe" && !scytheEquipped)
        {
            currentWeapon = Weapon;
            scytheEquipped = true;
            throwWeapon.enabled = true;
            emptyIM.SetActive(false);
            scytheIM.SetActive(true);
            currentWeapon.transform.position = gunPos2.position;
            currentWeapon.transform.parent = gunPos2;
            currentWeapon.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
            return;

        }
        
    }


    public void Drop()
    {
        if (currentWeapon == null || throwWeapon.isThrown || throwWeapon.isReturning) return;
        rotatorSwing.enabled = false;
        currentWeapon.GetComponent<Rigidbody>().isKinematic = false;
        currentWeapon.GetComponent<Rigidbody>().useGravity = true;
        throwWeapon.enabled = false;
        scytheEquipped = false;
        swordEquipped = false;
        swordIM.SetActive(false);
        scytheIM.SetActive(false);
        emptyIM.SetActive(true);
        currentWeapon.transform.parent = null;

        Collider col = currentWeapon.GetComponent<Collider>();
        if (col != null) col.enabled = true;
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

   
