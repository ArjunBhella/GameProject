using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform player; // Reference to the player
    public LayerMask whatIsGround, whatIsPlayer;
    public GameObject projectile;

    // Health and Damage
    public int maxHealth = 100;
    public int attackDamage = 20;
    private int currentHealth;
    public GameObject[] droppableItems;
    [Range(0, 100)] public int dropChance = 50;
    public float dropForce = 5f;

    // Movement and Attacking
    public float timeBetweenAttacks = 1.5f;
    public float speed = 3.0f; // Movement speed
    public float stopDistance = 4.0f; // Stopping distance from the player
    public float attackDistance = 2.0f; // Distance to start attacking the player

    private bool alreadyAttacked;

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
    }

    private void Update()
    {
        if (player == null) return;

        // Calculate distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Handle behavior based on distance
        if (distanceToPlayer <= attackDistance)
        {
            AttackPlayer(); // Play attack animation and handle attacking
        }
        else
        {
            FollowPlayerBehavior(distanceToPlayer); // Move towards the player if not in attack range
        }
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

        // Attack logic
        if (!alreadyAttacked)
        {
            // Placeholder for actual attack mechanics (e.g., projectile, melee)
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

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerMovement = collision.gameObject.GetComponent<CharacterMovement>();
            if (playerMovement != null)
            {
                playerMovement.TakeDamage(attackDamage);
            }
        }
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
