using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask GroundCheck, PlayerCheck;
    public GameObject projectile;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    private float stuckCheckTimer;
    public float stuckCheckInterval = 2f;
    private Vector3 lastPosition;

    // Attacking
    public float timeDelayAttacks, timeDelayBurst, burst;
    bool Attacked;

    // States
    public float sightDistance, attackDistance;
    public bool playerSeenDistance, playerAttackDistance;

    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 6f;
    private float defaultSpeed;


    private void Awake()
    {
        walkPointSet = false;
        defaultSpeed = patrolSpeed;
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
        playerSeenDistance = Physics.CheckSphere(transform.position, sightDistance, PlayerCheck);
        playerAttackDistance = Physics.CheckSphere(transform.position, attackDistance, PlayerCheck);

        if (!playerSeenDistance && !playerAttackDistance)
        {
            agent.speed = defaultSpeed;
            Patrolling();
        }
        if (playerSeenDistance && !playerAttackDistance)
        {
            agent.speed = chaseSpeed;
            ChasePlayer();
        }
        if (playerSeenDistance && playerAttackDistance)
        {
            agent.speed = chaseSpeed; 
            ChasePlayer();           
            AttackPlayer();
        }

        CheckIfStuck();
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

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
        for (int i = 0; i < 10; i++)
        {
            float randZ = Random.Range(-walkPointRange, walkPointRange);
            float randX = Random.Range(-walkPointRange, walkPointRange);

            Vector3 potentialPoint = new Vector3(
                transform.position.x + randX,
                transform.position.y + 5f,
                transform.position.z + randZ
            );

            if (Physics.Raycast(potentialPoint, Vector3.down, out RaycastHit hit, 20f, GroundCheck))
            {
                walkPoint = hit.point;
                walkPointSet = true;
                return;
            }
        }
    }

    private void ResetWalkPoint()
    {
        walkPointSet = false;
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 targetPosition = player.position - direction ;
        agent.SetDestination(targetPosition);
    }

    private void AttackPlayer()
    {
        transform.LookAt(player); 

        if (!Attacked)
        {
            burst++;
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            projectile.SetActive(true);
            rb.AddForce(transform.forward * 20f, ForceMode.Impulse);

            if (burst < 3f)
                Invoke(nameof(ResetAttack), timeDelayAttacks);
            else
            {
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

    private void CheckIfStuck()
    {
        stuckCheckTimer += Time.deltaTime;

        if (stuckCheckTimer >= stuckCheckInterval)
        {
            float distanceMoved = Vector3.Distance(transform.position, lastPosition);

            if (distanceMoved < 0.5f && !playerSeenDistance)
            {
                walkPointSet = false;
                SearchWalkPoint();
            }

            lastPosition = transform.position;
            stuckCheckTimer = 0f;
        }
    }
}
