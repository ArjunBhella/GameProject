using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float speed = 6.0f; // Running speed
    public float stopDistance = 2.0f; // Distance to stop from the player
    private Animator animator; // Reference to the Animator

    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        if (player == null) return;

        // Calculate the distance to the player
        float distance = Vector3.Distance(transform.position, player.position);

        // If further than the stopping distance, move and play running animation
        if (distance > stopDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Face the player
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

            // Trigger running animation
            if (animator != null)
            {
                animator.SetBool("isRunning", true);
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
}

