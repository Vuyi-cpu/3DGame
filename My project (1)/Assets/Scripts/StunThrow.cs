using System;
using TMPro;
using UnityEngine;

public class StunThrow : MonoBehaviour
{
    [SerializeField] GameObject stun; //Game object reference to scythe. Used to move scythe
    [SerializeField] Rigidbody stunRb;
    [SerializeField] Transform stunLocation; //Original scythe location
    [SerializeField] float throwSpeed; //Throw speed of this thing
    [SerializeField] LayerMask layerMask; //Layer mask for raycast check. Looking for environment layer
    [SerializeField] private Transform player; //Player transform

    public bool isThrown; //Bool that gets set when thrown
    [SerializeField] Vector3 throwPosition; //This is where the scythe is traveling to.
    [SerializeField] Rotator rotator; //Rotator on scythe object. Gets turned on when thrown. And off when not.
    public PlayerControls controls;
    public PlayerMovement PlayerMovement;
    public EnemyAI enemyAI;
    public MonoBehaviour[] scriptsToDisable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (!PlayerMovement.active)
        {
            controls.Player.Attack.performed += ctx =>
            {
                isThrown = true;
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isThrown)
        {
            throwPosition = player.transform.position + stunLocation.forward * 1000;
            stunRb.isKinematic = false;
            //Set new position to move towards and apply to scythe transform.
            Vector3 newPos = Vector3.MoveTowards(stun.transform.position, throwPosition, throwSpeed * Time.deltaTime);
            stun.transform.position = newPos;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(stun);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            foreach (MonoBehaviour script in scriptsToDisable)
            {
                if (enemyAI != null)
                    enemyAI.enabled = false;
            }

            // Wait for duration
            yield return new WaitForSeconds(3f);

            // Re-enable scripts
            foreach (MonoBehaviour script in scriptsToDisable)
            {
                if (enemyAI != null)
                    enemyAI.enabled = true;
            }
        }
    }
}
