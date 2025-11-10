using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossAI: MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    public LayerMask GroundCheck, PlayerCheck, obstructionMask;
    public GameObject projectile;
    public PlayerState state;

    // Patrolling
   
    private bool walkPointSet, hasLineOfSight;
    public float walkPointRange;
    public float  turnSpeed;

    // Attacking
    public float timeDelayAttacks, timeDelayBurst, burst;
    private bool Attacked;
    public bool flameShooting;
    public Transform firePos;

    // States
    public float sightDistance, attackDistance, rejectDistance;
    public ParticleSystem fire;
    public Collider flameCollider;


    public float chaseSpeed = 6f;
    private float defaultSpeed;
    public float distanceToPlayer;

    public AudioSource attack;
    public AudioSource flameflow;


    private void Awake()
    {
        flameCollider.enabled = false;
        flameShooting = false;
        if (fire != null) fire.Stop();
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
              
               // Patrolling();
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

       
        transform.LookAt(targetPos);
        if (!Attacked && gameObject.CompareTag("Boss"))
        {
            StartCoroutine(flameShoot());
            Invoke(nameof(ResetAttack), timeDelayAttacks);
            Attacked = true;
        }
    }


    IEnumerator flameShoot()
    {
        attack.Play();
        flameflow.Play();
        flameShooting = true;
        flameShooting = true;
        fire.transform.position = firePos.position;
        fire.transform.SetParent(firePos);
        fire.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        fire.Play();
        
        yield return new WaitForSeconds(0.7f);
        flameCollider.enabled = true;
        yield return new WaitForSeconds(2.3f);
        fire.Stop();
        attack.Stop();
        flameflow.Stop();
        fire.transform.SetParent(null);
        flameShooting = false;
        flameCollider.enabled = false; 
    }

    private void ResetAttack()
    {
        Attacked = false;
    }

}
