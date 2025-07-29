using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;
    public SelectionManager SelectionManager;
    public GameObject Weapon;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && SelectionManager.playerCanInteract == true)
        {
            Debug.Log("Item added to inventory");
            Destroy(gameObject);
            Weapon.SetActive(true);
        }
    }

    public string GetItemName()
    {
        return ItemName;
    }
}
