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
    public SelectionManager SelectionManager;
    GameObject Weapon;
    PlayerControls controls;
    [SerializeField] private ThrowWeapon throwWeapon;

    //vuyi edits
    public Transform gunPos;
    public Transform gunPos2;
    public float range = 10f;
    GameObject currentWeapon;

    public bool equipped;

    private Vector2 pickup;

    private void Awake()
    {
        throwWeapon.enabled = false;
        controls = new PlayerControls();
        controls.Player.Interact.performed += ctx =>
        {

            if ( SelectionManager.playerCanInteract == true)
            {
                if (currentWeapon == null) Pickup();
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

        }
    }
    
    private void Pickup()
    {
        currentWeapon = Weapon;
        equipped = true;
        if (currentWeapon.tag == "sword")
        {
            emptyIM.SetActive(false);
            swordIM.SetActive(true);
            currentWeapon.transform.position = gunPos.position;
            currentWeapon.transform.parent = gunPos;
            currentWeapon.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            currentWeapon.GetComponent<Rigidbody>().isKinematic = true;

        }
        else if (currentWeapon.tag == "scythe")
        {
            throwWeapon.enabled = true;
            emptyIM.SetActive(false);
            scytheIM.SetActive(true);
            currentWeapon.transform.position = gunPos2.position;
            currentWeapon.transform.parent = gunPos2;
            currentWeapon.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
        }
        

  
    }

    private void Drop()
    {
        throwWeapon.enabled = false;
        equipped = false;
        swordIM.SetActive(false);
        scytheIM.SetActive(false);
        emptyIM.SetActive(true);
        currentWeapon.transform.parent = null;
        currentWeapon.GetComponent<Rigidbody>().isKinematic = false;

        currentWeapon = null;
     
    }

    public string GetItemName()
    {
        return ItemName;
    }
}
