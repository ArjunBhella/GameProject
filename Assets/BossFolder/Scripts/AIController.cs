using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AIController : MonoBehaviour
{
    public Transform player;                        // Reference to the player
    public float sightRange = 15f;                  // Boss detects player within this range
    public float attackCooldown = 5f;               // Time between attacks

    [Header("Attack Probabilities")]
    [Range(0, 100)] public int knockUpChance = 50;  // % chance for Knock-Up Attack
    [Range(0, 100)] public int crashAttackChance = 50; // % chance for Crash Attack

    [Header("Knock-Up Attack")]
    public GameObject warningCirclePrefab;          // Prefab for the warning circle
    public GameObject magmaExplosionPrefab;         // Prefab for the magma explosion
    public float knockUpRadius = 1f;                // Radius of the knock-up attack
    public float knockUpHeight = 2f;                // Height of the knock-up effect

    [Header("Crash Attack")]
    public GameObject boulderPrefab;                // Boulder prefab
    public GameObject warningLinePrefab;            // Warning line prefab
    public float boulderSpawnDistance = 5f;         // Distance from the player where boulders spawn
    public float boulderSpeed = 10f;                // Speed at which boulders move
    public float warningDuration = 1.5f;            // Duration the warning is visible

    private NavMeshAgent agent;
    private Animator animator;
    private bool canAttack = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        RotateToFacePlayer();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= sightRange && canAttack)
        {
            StartCoroutine(PerformRandomAttack());
        }
    }

    void RotateToFacePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0; // Ignore vertical rotation

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
        }
    }

    IEnumerator PerformRandomAttack()
    {
        canAttack = false;

        // Randomly choose between Knock-Up Attack and Crash Attack
        int randomValue = Random.Range(0, 100);
        if (randomValue < knockUpChance)
        {
            StartCoroutine(PerformKnockUpAttack());
        }
        else
        {
            StartCoroutine(PerformCrashAttack());
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator PerformKnockUpAttack()
    {
        agent.isStopped = true;
        animator.SetTrigger("KnockUpAttack");

        Vector3 attackPosition = new Vector3(player.position.x, player.position.y, player.position.z);
        SpawnEffect(warningCirclePrefab, attackPosition, Quaternion.Euler(90, 0, 0), 1.0f);

        yield return new WaitForSeconds(1.0f);

        SpawnEffect(magmaExplosionPrefab, attackPosition, Quaternion.identity, 2.0f);

        float distanceToPlayer = Vector3.Distance(player.position, attackPosition);
        if (distanceToPlayer <= knockUpRadius)
        {
            if (player.TryGetComponent(out SimpleCharacterMovement playerMovement))
            {
                playerMovement.ApplyKnockUp(knockUpHeight);
            }
        }

        agent.isStopped = false;
    }

    IEnumerator PerformCrashAttack()
    {
        agent.isStopped = true;
        animator.SetTrigger("CrashAttack");

        // Calculate spawn positions
        Vector3 leftSpawnPos = player.position + (player.right * -boulderSpawnDistance);
        Vector3 rightSpawnPos = player.position + (player.right * boulderSpawnDistance);

        // Calculate the fixed target position (player's current position at the start of the attack)
        Vector3 fixedTargetPosition = player.position;

        // Show warning line
        Vector3 linePosition = (leftSpawnPos + rightSpawnPos) / 2f;
        Vector3 lineScale = new Vector3(boulderSpawnDistance * 2, 0.1f, 1f);

        GameObject warningLine = Instantiate(warningLinePrefab, linePosition, Quaternion.Euler(90, 0, 0));
        warningLine.transform.localScale = lineScale;

        yield return new WaitForSeconds(warningDuration);
        Destroy(warningLine);

        // Spawn boulders
        GameObject leftBoulder = Instantiate(boulderPrefab, leftSpawnPos, Quaternion.identity);
        GameObject rightBoulder = Instantiate(boulderPrefab, rightSpawnPos, Quaternion.identity);

        float timer = 0f;

        while (timer < 2f) // Time to move boulders
        {
            timer += Time.deltaTime;

            // Move boulders toward the FIXED target position
            leftBoulder.transform.position = Vector3.MoveTowards(
                leftBoulder.transform.position, fixedTargetPosition, boulderSpeed * Time.deltaTime);
            rightBoulder.transform.position = Vector3.MoveTowards(
                rightBoulder.transform.position, fixedTargetPosition, boulderSpeed * Time.deltaTime);

            yield return null;
        }

        Destroy(leftBoulder, 1.0f);
        Destroy(rightBoulder, 1.0f);

        agent.isStopped = false;
    }


    void SpawnEffect(GameObject prefab, Vector3 position, Quaternion rotation, float duration)
    {
        if (prefab != null)
        {
            GameObject effectInstance = Instantiate(prefab, position, rotation);
            Destroy(effectInstance, duration);
        }
    }
}
