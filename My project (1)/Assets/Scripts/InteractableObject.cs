using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject emptyIM;
    public GameObject swordIM;
    public string ItemName;
    public SelectionManager SelectionManager;
    public GameObject Weapon;

    private void Update()
    {
        swordIM.SetActive(false);
        if (Input.GetKeyDown(KeyCode.E) && SelectionManager.playerCanInteract == true)
        {
            Debug.Log("Item added to inventory");
            Destroy(gameObject);
            emptyIM.SetActive(false);
            swordIM.SetActive(true);
            Weapon.SetActive(true);
        }
    }

    public string GetItemName()
    {
        return ItemName;
    }
}
