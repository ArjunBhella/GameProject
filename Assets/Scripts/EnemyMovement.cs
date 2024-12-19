using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target; // The player
    public float speed = 3f; // Speed of the enemy
    private Animator animator;
    private bool isTouchingPlayer = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isTouchingPlayer)
        {
            // Start repeating the attack animation
            animator.SetBool("isRunning", false);
            if (!IsInvoking("ToggleAttackAnimation"))
            {
                InvokeRepeating("ToggleAttackAnimation", 0f, 1f);  // Start toggling every 1 second
            }
        }
        else
        {
            // Stop repeating the attack animation
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", false);
            CancelInvoke("ToggleAttackAnimation");

            // Move towards the player if not touching
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > 1f)
            {
                animator.SetBool("isRunning", true);
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
        }
    }

    // This method will toggle the attack animation
    void ToggleAttackAnimation()
    {
        animator.SetBool("isAttacking", false); // Stop the attack animation
        animator.SetBool("isAttacking", true);  // Restart the attack animation
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = true; // Start attacking when the enemy touches the player
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = false; // Stop attacking when the enemy stops touching the player
        }
    }
}


