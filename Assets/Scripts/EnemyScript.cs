using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform player; // Reference to the player
    public LayerMask whatIsGround, whatIsPlayer;
    public GameObject projectile;
    public GameObject bloodEffectPrefab; // Reference to the blood effect prefab

    // Health and Damage
    public int maxHealth = 100;
    public int attackDamage = 20;
    private int currentHealth;
    public GameObject[] droppableItems;
    [Range(0, 100)] public int dropChance = 50;
    public float dropForce = 5f;

    // Movement and Attacking
    public float timeBetweenAttacks = 0.5f;
    public float speed = 3.0f; // Movement speed
    public float stopDistance = 4.0f; // Stopping distance from the player
    public float attackDistance = 2.0f; // Distance to start attacking the player
    public float detectionDistance = 20.0f; // Distance to detect the player
    public float patrolSpeed = 1.5f; // Speed while patrolling
    public float patrolTime = 3.0f; // Time for each patrol action

    private bool alreadyAttacked;
    private bool isPatrolling = true;
    private float patrolTimer;
    private Vector3 patrolTarget;

    // Animation
    private Animator animator;

    private Rigidbody rb;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        // Freeze rotation to prevent spinning due to physics forces
        rb.freezeRotation = true;

        // Initialize patrol timer
        patrolTimer = patrolTime;
        SetRandomPatrolTarget();
    }

    private void Update()
    {
        if (player == null) return;

        // Calculate distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionDistance)
        {
            // Player detected, stop patrolling and chase the player
            isPatrolling = false;
            if (distanceToPlayer <= attackDistance)
            {
                AttackPlayer(); // Attack logic if within attack range
            }
            else
            {
                FollowPlayerBehavior(distanceToPlayer); // Chase player
            }
        }
        else
        {
            // Player not detected, patrol
            if (!isPatrolling)
            {
                isPatrolling = true; // Start patrolling again
                animator.SetBool("isRunning", false);
                animator.SetBool("isAttacking", false);
            }
            Patrol();
        }
    }

    private void Patrol()
    {
        if (patrolTimer <= 0)
        {
            // Set a new random patrol target and reset the timer
            SetRandomPatrolTarget();
            patrolTimer = patrolTime;
        }
        else
        {
            patrolTimer -= Time.deltaTime;

            // Move towards the patrol target
            Vector3 direction = (patrolTarget - transform.position).normalized;
            transform.position += direction * patrolSpeed * Time.deltaTime;

            // Face the patrol target
            transform.LookAt(new Vector3(patrolTarget.x, transform.position.y, patrolTarget.z));

            // Play walking animation
            animator.SetBool("isRunning", true);
            animator.SetBool("isAttacking", false);

            // Check if close enough to the target to stop moving
            if (Vector3.Distance(transform.position, patrolTarget) < 1.0f)
            {
                patrolTimer = 0; // Force a new patrol target on the next update
            }
        }
    }

    private void SetRandomPatrolTarget()
    {
        // Choose a random direction and distance for patrol
        Vector3 randomDirection = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)).normalized;
        patrolTarget = transform.position + randomDirection * Random.Range(5, 10); // Random distance between 5 and 10 units
    }

    private void FollowPlayerBehavior(float distanceToPlayer)
    {
        if (distanceToPlayer > stopDistance)
        {
            // Move towards the player
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Face the player
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

            // Trigger running animation
            if (animator != null)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isAttacking", false);
            }
        }
        else
        {
            // Stop running animation
            if (animator != null)
            {
                animator.SetBool("isRunning", false);
            }
        }
    }

    private void AttackPlayer()
    {
        if (animator != null)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", true);
        }

        // Face the player
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        // Trigger the blood effect at the player's position
        Vector3 bloodEffectPosition = player.position + Vector3.up * 1.5f; // Adjust height as needed
        GameObject bloodEffect = Instantiate(bloodEffectPrefab, bloodEffectPosition, Quaternion.identity);

        // Destroy the blood effect after it plays
        Destroy(bloodEffect, 2.0f); // Adjust lifespan to match particle effect duration

        // Attack logic
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;

            // Trigger the attack and reset after the cooldown
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        // Perform your attack action here (e.g., instantiate a projectile or apply damage)
        Debug.Log("Enemy attacking!");

        // Wait for the attack cooldown
        yield return new WaitForSeconds(timeBetweenAttacks);

        // Reset attack flag to allow the next attack
        alreadyAttacked = false;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (Random.Range(0, 100) < dropChance && droppableItems.Length > 0)
        {
            GameObject itemToDrop = droppableItems[Random.Range(0, droppableItems.Length)];
            GameObject droppedItem = Instantiate(itemToDrop, transform.position, Quaternion.identity);
            Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 dropDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
                rb.AddForce(dropDirection * dropForce, ForceMode.Impulse);
            }
        }
        Destroy(gameObject);
    }
}