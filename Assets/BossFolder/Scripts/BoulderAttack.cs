using UnityEngine;
using System.Collections;

public class BoulderAttack : MonoBehaviour
{
    public Transform player;                    // Reference to the player
    public GameObject boulderPrefab;            // Boulder Prefab
    public GameObject warningLinePrefab;        // Warning line Prefab

    public float spawnDistance = 5f;            // Distance from the player where boulders spawn
    public float boulderSpeed = 10f;            // Speed at which boulders move
    public float warningDuration = 1.5f;        // Duration the warning line is visible
    public float boulderDelay = 2f;             // Delay before boulders start moving

    private GameObject warningLineInstance;     // Instance of the warning line

    void Start()
    {
        StartCoroutine(SpawnBouldersAttack());
    }

    IEnumerator SpawnBouldersAttack()
    {
        // Step 1: Calculate spawn positions for the boulders
        Vector3 leftSpawnPos = player.position + (player.right * -spawnDistance);
        Vector3 rightSpawnPos = player.position + (player.right * spawnDistance);

        // Step 2: Spawn the warning line
        Vector3 linePosition = (leftSpawnPos + rightSpawnPos) / 2f;
        Vector3 lineScale = new Vector3(spawnDistance * 2, 0.1f, 1f); // Stretch line between boulders

        warningLineInstance = Instantiate(warningLinePrefab, linePosition, Quaternion.Euler(90, 0, 0));
        warningLineInstance.transform.localScale = lineScale;

        yield return new WaitForSeconds(warningDuration); // Wait for warning duration

        // Destroy the warning line
        Destroy(warningLineInstance);

        // Step 3: Spawn the boulders
        GameObject leftBoulder = Instantiate(boulderPrefab, leftSpawnPos, Quaternion.identity);
        GameObject rightBoulder = Instantiate(boulderPrefab, rightSpawnPos, Quaternion.identity);

        // Step 4: Move the boulders towards the center
        Vector3 targetPosition = player.position; // Target is roughly the player position

        float timer = 0f;
        while (timer < boulderDelay)
        {
            timer += Time.deltaTime;

            // Move boulders toward the target
            leftBoulder.transform.position = Vector3.MoveTowards(
                leftBoulder.transform.position, targetPosition, boulderSpeed * Time.deltaTime);
            rightBoulder.transform.position = Vector3.MoveTowards(
                rightBoulder.transform.position, targetPosition, boulderSpeed * Time.deltaTime);

            yield return null;
        }

        // Step 5: Destroy the boulders after collision
        Destroy(leftBoulder, 1.0f);
        Destroy(rightBoulder, 1.0f);
    }
}
