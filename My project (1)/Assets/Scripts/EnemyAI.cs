using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask GroundCheck, PlayerCheck;
    public GameObject projectile;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeDelayAttacks, timeDelayBurst, burst;
    bool Attacked;

    //States
    public float sightDistance, attackDistance;
    public bool playerSeenDistance, playerAttackDistance;

    private void Awake()
    {
        agent.updateRotation = false; 
        agent.updateUpAxis = false;
        walkPointSet = false;
    }

    private void Update()
    {
        playerSeenDistance = Physics.CheckSphere(transform.position, sightDistance, PlayerCheck);
        playerAttackDistance = Physics.CheckSphere(transform.position, attackDistance, PlayerCheck);
        if (!playerSeenDistance && !playerAttackDistance) Patrolling();
        if (playerSeenDistance && !playerAttackDistance) agent.SetDestination(player.transform.position); ;
        if (playerSeenDistance && playerAttackDistance) AttackPlayer();
    }
    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;
            if (distanceToWalkPoint.magnitude < 1f)
            {
                Invoke(nameof(ResetWalkPoint), Random.Range(1f, 3f));
            }
        }
    }

    private void SearchWalkPoint()
    {
        float randZ = Random.Range(-walkPointRange, walkPointRange);
        float randX = Random.Range(-walkPointRange, walkPointRange);

        Vector3 potentialPoint = new Vector3(
            transform.position.x + randX,
            transform.position.y + 5f,
            transform.position.z + randZ
        );

        // Cast down to find the ground
        if (Physics.Raycast(potentialPoint, Vector3.down, out RaycastHit hit, 20f, GroundCheck))
        {
            walkPoint = hit.point;
            walkPointSet = true;
            Debug.Log("WalkPoint set");
        }
    }

    private void ResetWalkPoint()
    {
        walkPointSet = false; 
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        if(!Attacked)
        {
            burst++;
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            projectile.SetActive(true);
            rb.AddForce(transform.forward * 20f, ForceMode.Impulse);
            if (burst < 3f) Invoke(nameof(ResetAttack), timeDelayAttacks);
            else { 
                Invoke(nameof(ResetAttack), timeDelayBurst);
                burst = 0;
            }
                Attacked = true;
        }
    }

    private void ResetAttack()
    {
        Attacked = false;
    }
}
