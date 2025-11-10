using System.Collections;
using Unity.VisualScripting;
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
    private bool walkPointSet, hasLineOfSight;
    public float walkPointRange;
    private float stuckCheckTimer;
    public float stuckCheckInterval = 2f, turnSpeed;
    private Vector3 lastPosition;

    // Attacking
    public float timeDelayAttacks, timeDelayBurst, burst;
    private bool Attacked;
    public bool flameShooting;
    public Transform firePos;

    // States
    public float sightDistance, attackDistance, rejectDistance;
    public ParticleSystem fire;
    public Collider flameCollider;

    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 6f;
    private float defaultSpeed;
    public float distanceToPlayer;

    public AudioSource attack;
    public AudioSource flameflow;
    private Animator dkAnimator;


    private void Awake()
    {
        flameCollider.enabled = false;
        flameShooting = false;
        if (fire != null) fire.Stop();
        agent = GetComponent<NavMeshAgent>();
        if (gameObject.CompareTag("Daisuke")) dkAnimator = GetComponentInChildren<Animator>();
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

        distanceToPlayer = Vector3.Distance(transform.position, player.position);


        hasLineOfSight = false;
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        if (Physics.Raycast(transform.position + Vector3.up * 1f, directionToPlayer, out RaycastHit hit, sightDistance + 40f))
        {
            if (hit.transform.CompareTag("Player"))
            {
                hasLineOfSight = true;
            }
        }

        if ((!flameShooting || !gameObject.CompareTag("Daisuke")) && hasLineOfSight)
        {
            if (distanceToPlayer > sightDistance)
            {
                agent.speed = patrolSpeed;
                Patrolling();
               
            }
            else if (distanceToPlayer > attackDistance)
            {
                agent.speed = chaseSpeed;
                ChasePlayer();
            }
            else if (distanceToPlayer <= attackDistance)
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
        }
        else if (hasLineOfSight)
        {
            Vector3 targetDir = player.position - transform.position;
            targetDir.y = 0f;
            if (targetDir.magnitude > 0.1f)
            {
                // Target rotation
                Quaternion targetRotation = Quaternion.LookRotation(targetDir);

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    turnSpeed * Time.deltaTime
                );
            }
        }

        if(agent == null)
        {
            flameflow.Stop();
            attack.Stop();
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

        //if (!gameObject.CompareTag("Daisuke") || Attacked) 
        transform.LookAt(targetPos);
        if (!Attacked && gameObject.CompareTag("Shooter"))
        {
            burst++;
            Vector3 enemyGun = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
            Rigidbody rb = Instantiate(projectile, enemyGun, Quaternion.identity).GetComponent<Rigidbody>();
            projectile.SetActive(true);
            rb.AddForce(transform.forward * 20f, ForceMode.Impulse);
            attack.Play();

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
            glint[0].Play();
            attack.Play();
            Invoke(nameof(ResetAttack), timeDelayAttacks);
            if (hasLineOfSight && distanceToPlayer <= attackDistance)
            {
                state.takeDamage(20f);
            }
            Attacked = true;
        }
        else if (!Attacked && gameObject.CompareTag("Daisuke"))
        {
            StartCoroutine(flameShoot());
            Invoke(nameof(ResetAttack), timeDelayAttacks);
            Attacked = true;
        }
    }


    IEnumerator flameShoot()
    {
        
        dkAnimator.SetBool("isAttacking", true);
        attack.Play();
        flameflow.Play();
        flameShooting = true;
        flameShooting = true;
        fire.transform.position = firePos.position;
        fire.transform.SetParent(firePos);
        fire.transform.localEulerAngles = new Vector3(0f, 0, 0f);
        fire.transform.localScale = new Vector3(1f, 1f, 2f);
        fire.Play();
        
        yield return new WaitForSeconds(0.7f);
        flameCollider.enabled = true;
        yield return new WaitForSeconds(2.3f);
        fire.Stop();
        attack.Stop();
        flameflow.Stop();
        fire.transform.SetParent(null);
        flameShooting = false;
        dkAnimator.SetBool("isAttacking", false);
        flameCollider.enabled = false; // disables the collider
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

            if (distanceMoved < 0.5f && distanceToPlayer > sightDistance)
            {
                walkPointSet = false;
                SearchWalkPoint();
            }

            lastPosition = transform.position;
            stuckCheckTimer = 0f;
        }
    }

}

