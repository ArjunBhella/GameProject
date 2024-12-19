using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement movement;

    
    public void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<CharacterMovement>();
    }
    public void LateUpdate()
    {
       UpdateAnimator();
    }

    // TODO Fill this in with your animator calls
    void UpdateAnimator()
    {
        animator.SetFloat("CharacterSpeed",movement.GetAnimationSpeed());
        animator.SetBool("IsFalling",!movement.isGrounded);
        if(movement.doubleJump){
            animator.SetTrigger("IsFlipping");
        }
        animator.SetBool("IsDashing",movement.isDashing);

    }


}
