using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
   
    public TMP_Text objectiveText; 

    
    public DoorRotate doorRotate1;
    public DoorRotate doorRotate2;

    private bool objectiveUpdated = false;
    private Quaternion door1StartRot;
    private Quaternion door2StartRot;

    void Start()
    {
        if (objectiveText == null)
        {
            Debug.Log("Objective Text not assigned in the Inspector!");
            return;
        }

        SetObjective("Find a way out the lab.");

        if (doorRotate1 != null) door1StartRot = doorRotate1.transform.rotation;
        if (doorRotate2 != null) door2StartRot = doorRotate2.transform.rotation;
    }

    void Update()
    {
        if (!objectiveUpdated)
        {
            bool door1Opened = doorRotate1 != null && doorRotate1.transform.rotation != door1StartRot;
            bool door2Opened = doorRotate2 != null && doorRotate2.transform.rotation != door2StartRot;

            if (door1Opened || door2Opened)
            {
                SetObjective("Defeat the Automatons.");
                objectiveUpdated = true; 
            }
        }
    }

    public void SetObjective(string newObjective)
    {
        if (objectiveText != null)
        {
            objectiveText.text = newObjective;
        }
    }
}
