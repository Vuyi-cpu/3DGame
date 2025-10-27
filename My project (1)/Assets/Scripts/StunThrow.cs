using System;
using TMPro;
using UnityEngine;

public class StunThrow : MonoBehaviour
{
    [SerializeField] GameObject stun; //Game object reference to scythe. Used to move scythe
    [SerializeField] Rigidbody stunRb;
    [SerializeField] Transform stunLocation; //Original scythe location
    [SerializeField] float throwSpeed, stunDistance; //Throw speed of this thing
    [SerializeField] LayerMask layerMask; //Layer mask for raycast check. Looking for environment layer
    [SerializeField] private Transform player; //Player transform

    public bool isThrown; //Bool that gets set when thrown
    [SerializeField] Vector3 throwPosition; //This is where the scythe is traveling to.
    [SerializeField] Rotator rotator; //Rotator on scythe object. Gets turned on when thrown. And off when not.
    public PlayerMovement PlayerMovement;
    public EnemyAI enemyAI;
    public MonoBehaviour[] scriptsToDisable;
    PlayerControls controls;
    InteractableObject stunInteract;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        stunInteract = GetComponent<InteractableObject>();
        controls = new PlayerControls();
        Debug.Log("awake set");
        controls.Player.Attack.performed += ctx =>
        {
            if (isThrown) return;
            throwStun();
            Debug.Log("throwing");
        };
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (isThrown)
        {
            stunRb.isKinematic = false;
            stunRb.useGravity = true;
            //Set new position to move towards and apply to scythe transform.
            Vector3 newPos = Vector3.MoveTowards(stun.transform.position, throwPosition, throwSpeed * Time.deltaTime);
            stun.transform.position = newPos;
            if (stun.transform.position == throwPosition) Destroy(stun);
        }
    }

    public void throwStun()
    {
        stunLocation.localEulerAngles = new Vector3(0f, 0f, 0f);
        throwPosition = player.transform.position + stunLocation.forward * stunDistance;
        stun.transform.parent = null;
        rotator.enabled = true;
        isThrown = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (stunInteract.stunEquipped)
        {
            stun.SetActive(false);
            InteractableObject.
        }
        
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        foreach (MonoBehaviour script in scriptsToDisable)
    //        {
    //            if (enemyAI != null)
    //                enemyAI.enabled = false;
    //        }

    //        // Wait for duration
    //        yield return new WaitForSeconds(3f);

    //        // Re-enable scripts
    //        foreach (MonoBehaviour script in scriptsToDisable)
    //        {
    //            if (enemyAI != null)
    //                enemyAI.enabled = true;
    //        }
    //    }
    }
}
