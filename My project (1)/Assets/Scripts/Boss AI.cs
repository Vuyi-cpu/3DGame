using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossAI: MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    public LayerMask GroundCheck, PlayerCheck, obstructionMask;
    public PlayerState state;

    // Patrolling
   
    private bool walkPointSet, hasLineOfSight;
    public float walkPointRange;
    public float  turnSpeed;

    // Attacking
    public float timeDelayAttacks, timeDelayBurst, burst;
    private bool Attacked;
    public bool flameShooting;

    // States
    public Collider flameCollider;


    public float chaseSpeed = 6f;
    private float defaultSpeed;
    public float distanceToPlayer;


    private void Awake()
    {
        flameCollider.enabled = false;
        flameShooting = false;
        agent = GetComponent<NavMeshAgent>();
        walkPointSet = false;
     
        agent.speed = defaultSpeed;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        agent.acceleration = 40f;
        agent.angularSpeed = 999f;
    }

    private void Update()
    {

        distanceToPlayer = Vector3.Distance(transform.position, player.position);


        hasLineOfSight = false;
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        if (Physics.Raycast(transform.position + Vector3.up * 1f, directionToPlayer, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Player"))
            {
                hasLineOfSight = true;
            }
        }

        if (hasLineOfSight)
        {
            Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(targetPos);
            StartCoroutine(AttackPlayer());
        }


    }


    private IEnumerator AttackPlayer()
    {
        yield return new WaitForSeconds(0.8f);
        flameCollider.enabled = true;
        yield return new WaitForSeconds(2.3f);
        flameCollider.enabled = true;
    }
}
