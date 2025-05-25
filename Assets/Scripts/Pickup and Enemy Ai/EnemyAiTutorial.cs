using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer, foodLayer;

    public float health;

    [Header("Animator")]
    public Animator animator;

    [Header("Bluffing & Attack")]
    public float bluffingRange;
    public float attackRange;
    public GameObject attackDeadzone;
    public GameObject resetDeadzone;

    [Header("Wandering")]
    public float walkPointRange;
    private Vector3 walkPoint;
    private bool walkPointSet;

    private bool playerInBluffingRange, playerInAttackRange;
    private bool hasFoodTarget = false;
    private bool foodConsumed = false;
    private Transform targetFood;

    private bool isChasing = false;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        if (attackDeadzone != null) attackDeadzone.SetActive(false);
        Debug.Log("Enemy AI Initialized.");
    }

    private void Update()
    {
        // Always lock X rotation to -89
        LockXRotation();

        if (foodConsumed)
        {
            agent.ResetPath();
            animator.SetBool("isFlying", false);
            animator.SetBool("isChasing", false);
            animator.SetBool("isIdle", true);
            return;
        }

        if (hasFoodTarget && targetFood != null)
        {
            agent.SetDestination(targetFood.position);
            return;
        }

        playerInBluffingRange = Physics.CheckSphere(transform.position, bluffingRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInAttackRange)
        {
            if (!attackDeadzone.activeSelf)
            {
                attackDeadzone.SetActive(true);
                Debug.Log("Attack deadzone activated.");
            }

            animator.SetBool("isFlying", true);
            LockXRotation(); // reinforce lock on transition
            animator.SetBool("isChasing", true);
            animator.SetBool("isIdle", false);
            agent.SetDestination(player.position);
            isChasing = true;
        }
        else
        {
            if (attackDeadzone.activeSelf)
            {
                attackDeadzone.SetActive(false);
                Debug.Log("Attack deadzone deactivated.");
            }

            if (isChasing)
            {
                isChasing = false;
                animator.SetBool("isChasing", false);
                animator.SetBool("isIdle", true);
                Debug.Log("Stopped chasing.");
            }

            if (playerInBluffingRange)
            {
                Bluffing();
            }
            else
            {
                animator.SetBool("isFlying", false);
                animator.SetBool("isIdle", true);
                SearchWalkPointIfNeeded();
            }
        }
    }

    private void Bluffing()
    {
        agent.ResetPath();
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        animator.SetBool("isFlying", true);
        LockXRotation(); // reinforce lock on transition
        animator.SetBool("isIdle", false);
    }

    private void LockXRotation()
    {
        transform.rotation = Quaternion.Euler(-89f, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    private void SearchWalkPointIfNeeded()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            if (Vector3.Distance(transform.position, walkPoint) < 1f)
                walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((foodLayer.value & (1 << other.gameObject.layer)) > 0 && !foodConsumed)
        {
            targetFood = other.transform;
            hasFoodTarget = true;
        }

        if (other.CompareTag("Player") && resetDeadzone != null && other.bounds.Intersects(resetDeadzone.GetComponent<Collider>().bounds))
        {
            Debug.Log("Player collided with reset deadzone.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (hasFoodTarget && targetFood != null && other.transform == targetFood)
        {
            Destroy(targetFood.gameObject);
            agent.ResetPath();
            foodConsumed = true;
            animator.SetBool("isFlying", false);
            animator.SetBool("isChasing", false);
            animator.SetBool("isIdle", true);
        }
    }
}
