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
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
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
        if (!walkPointSet) SearchWalkPoint();
        else { agent.SetDestination(walkPoint); }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randZ = Random.Range(-walkPointRange, walkPointRange);
        float randX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randX, transform.position.y, transform.position.z + randZ);
        if(Physics.Raycast(walkPoint, -transform.up, 2f, GroundCheck)) walkPointSet = true;
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
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
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
