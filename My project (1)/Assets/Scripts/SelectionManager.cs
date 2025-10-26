using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public bool playerCanInteract = false;
    public GameObject interaction_Info_UI;
    TextMeshProUGUI interaction_text;
    public InteractableObject key;
    public InteractableObject scythe;
    public InteractableObject katana;
    public InteractableObject telephone;

    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            if (selectionTransform.GetComponent<InteractableObject>() && hit.distance < 3)
            {
                playerCanInteract = true;
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_Info_UI.SetActive(true);
            }
            else
            {
                key.isKey = false;
                scythe.isKey = false;
                katana.isKey = false;
                telephone.isKey = false;
                playerCanInteract = false;
                interaction_Info_UI.SetActive(false);
            }
        }
        else
        {
            interaction_Info_UI.SetActive(false);
        }
    }

    
}