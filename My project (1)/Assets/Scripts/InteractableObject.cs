using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject emptyIM;
    public GameObject swordIM;
    public GameObject sytheIM;
    public string ItemName;
    public SelectionManager SelectionManager;
    GameObject Weapon;


    //vuyi edits
    public Transform gunPos;
    public float range = 10f;
    GameObject currentWeapon;
  

    public bool equipped;

    public void Start()
    {
        swordIM.SetActive(false);
        sytheIM.SetActive(false);
    }

    private void Update()
    {
        CheckWeapons();
        
        if (Input.GetKeyDown(KeyCode.E)&&SelectionManager.playerCanInteract == true)
        {
                if (currentWeapon == null)
                Pickup();
            
        }

        if (currentWeapon != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                Drop();
        }
    }

    private void CheckWeapons()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            if (hit.transform.tag == "sword")
            {
                Debug.Log("can interact");
                Weapon = hit.transform.gameObject;
            }else if(hit.transform.tag == "sythe")
            {
                Debug.Log("can interact");
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

        } else if (currentWeapon.tag == "sythe")
        {
            emptyIM.SetActive(false);
            sytheIM.SetActive(true);
            currentWeapon.transform.position = gunPos.position;
            currentWeapon.transform.parent = gunPos;
            currentWeapon.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
        }


  
    }

    private void Drop()
    {
        equipped = false;
        swordIM.SetActive(false);
        sytheIM.SetActive(false);
        emptyIM.SetActive(true);
        currentWeapon.transform.parent = null;
        currentWeapon.GetComponent<Rigidbody>().isKinematic = false;
        currentWeapon = null;
    }




    //end vuyi edits

    //private void Update()
    //{
    //  
    //    if (Input.GetKeyDown(KeyCode.E) && SelectionManager.playerCanInteract == true)
    //    {
    //        Debug.Log("Item added to inventory");
    //        Destroy(gameObject);
    //        
    //     
    //        Weapon.SetActive(true);
    //    }
    //}

    public string GetItemName()
    {
        return ItemName;
    }
}
