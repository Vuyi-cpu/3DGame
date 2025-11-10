using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    public LayerMask GroundCheck, PlayerCheck, obstructionMask;
    public GameObject projectile;
    public PlayerState state;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    private float stuckCheckTimer;
    public float stuckCheckInterval = 2f, turnSpeed;
    private Vector3 lastPosition;

    // Attacking
    public float timeDelayAttacks, timeDelayBurst, burst;
    bool Attacked;

    // States
    public float sightDistance, attackDistance, rejectDistance;
    public bool playerSeenDistance, playerAttackDistance, playerRejectDistance;
    private ParticleSystem fire;
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 6f;
    private float defaultSpeed;

    public AudioSource gunshot;
    

    private void Awake()
    {
        if (transform.parent != null)
        {
            fire = transform.parent.GetComponentInChildren<ParticleSystem>();
            if (fire != null) fire.Stop();
        }

        if (fire != null ) fire.Stop();
        agent = GetComponent<NavMeshAgent>();
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
      
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

      
        bool hasLineOfSight = false;
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        if (Physics.Raycast(transform.position + Vector3.up * 1f, directionToPlayer, out RaycastHit hit, sightDistance + 40f))
        {
            if (hit.transform.CompareTag("Player"))
            {
                hasLineOfSight = true;
            }
        }

       
        if (hasLineOfSight && distanceToPlayer > sightDistance)
        {
            if (gameObject.CompareTag("Daisuke"))
            {
                agent.speed = chaseSpeed;
                ChasePlayer();
            }
            else
            {
                agent.speed = patrolSpeed;
                Patrolling();
            }
        }
        else if (hasLineOfSight && distanceToPlayer > attackDistance)
        {
           
            agent.speed = chaseSpeed;
            ChasePlayer();
        }
        else if (hasLineOfSight && distanceToPlayer <= attackDistance)
        {
           
            agent.SetDestination(transform.position); 
            if (distanceToPlayer < rejectDistance)
            {
             
                Vector3 dirToPlayer = (transform.position - player.position).normalized;
                Vector3 targetPos = player.position + dirToPlayer * rejectDistance;
                agent.SetDestination(targetPos);
            }
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
        Vector3 targetPosition = player.position - direction;
        agent.SetDestination(targetPosition);
    }

    private void AttackPlayer()
    {
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);

        //if (gameObject.CompareTag("Shooter") || gameObject.CompareTag("Melee") || Attacked) 
            transform.LookAt(targetPos);
        if (!Attacked && gameObject.CompareTag("Shooter"))
        {
            burst++;
            Vector3 enemyGun = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
            Rigidbody rb = Instantiate(projectile, enemyGun, Quaternion.identity).GetComponent<Rigidbody>();
            projectile.SetActive(true);
            rb.AddForce(transform.forward * 20f, ForceMode.Impulse);
            gunshot.Play();

            if (burst < 3f)
                Invoke(nameof(ResetAttack), timeDelayAttacks);
            else
            {
                Invoke(nameof(ResetAttack), timeDelayBurst);
                burst = 0;
            }
            Attacked = true;
        }
        else if (!Attacked && gameObject.CompareTag("Melee"))
        {
            ParticleSystem[] glint = GetComponentsInChildren<ParticleSystem>();
            Invoke(nameof(ResetAttack), timeDelayAttacks);
            glint[0].Play();
            glint[1].Play();
            Invoke(nameof(ResetAttack), timeDelayAttacks);
            if (playerSeenDistance && playerAttackDistance)
            {
                state.takeDamage();
            }
            Attacked = true;
        }
        else if (!Attacked && gameObject.CompareTag("Daisuke"))
        {
            fire.Play();
            /*if (targetPos.magnitude > 0.1f)
            {
                // Target rotation
                Quaternion targetRotation = Quaternion.LookRotation(targetPos);

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    turnSpeed * Time.deltaTime
                );
            }*/
            Invoke(nameof(ResetAttack), timeDelayAttacks);
            fire.Stop();
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
    bool HasLineOfSight()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        if (Physics.Raycast(transform.position + Vector3.up * 1f, dir, out RaycastHit hit, sightDistance))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

}

