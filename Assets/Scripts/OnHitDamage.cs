using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitDamage : MonoBehaviour
{
    public CharacterMovement controller;

    void Start()
    {
        // Find the CharacterMovement script on the player game object
        controller = FindObjectOfType<CharacterMovement>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the tag "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Try to get the EnemyScript component
            EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();

            if (enemy != null)
            {
                // Determine damage based on whether a special attack was used
                int damageToApply = controller.canUseSpecial ? controller.specialAttackDamage : controller.punchDamage;

                // Apply damage to the enemy
                enemy.TakeDamage(damageToApply);

                // Reset the special attack flag
                if (controller.canUseSpecial)
                {
                    controller.canUseSpecial = false;
                    Debug.Log("Special attack used, resetting flag.");
                }
            }
        }
    }
}
