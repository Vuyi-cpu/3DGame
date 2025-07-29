using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;
    public SelectionManager SelectionManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && SelectionManager.playerCanInteract == true)
        {
            Debug.Log("Item added to inventory");
            Destroy(gameObject);
        }
    }

    public string GetItemName()
    {
        return ItemName;
    }
}
