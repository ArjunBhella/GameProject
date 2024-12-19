using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
    public Transform cameraTransform; 

    public ParticleSystem leftPunchEffect;
    public ParticleSystem rightPunchEffect;
    public AudioSource audioSource;
    public AudioClip punchSound; 
    public AudioClip damageSound;
    public AudioClip specialAttackSound;
    Vector3 playerVelocity;
    Vector3 move;
    Vector3 startPosition;

    public float walkSpeed = 5;
    public float runSpeed = 8;
    public float jumpHeight = 1;
    public int maxJumpCount = 1;
    public float gravity = -9.18f;
    public bool isGrounded;
    public bool isRunning;
    private CharacterController controller;
    private Animator animator;

    public float coolDownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;

    private bool canDoubleJump;
    public bool doubleJump;
    private float doubleJumpDuration;
    private float doubleJumpTimer;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1.0f;
    public bool isDashing = false;
    private float dashTimeRemaining;
    private float dashCooldownRemaining;

      public Collider leftHandCollider;
    public Collider rightHandCollider;
    

    public bool canUseSpecial = true; 
    private float specialAttackCooldown = 60f; 
    public float specialAttackTimer = 0;

    public int punchDamage = 20;
     public int specialAttackDamage = 50;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        doubleJumpTimer = 0f;
        canDoubleJump = false;
        dashCooldownRemaining = 0f;
        leftHandCollider.enabled = false;
        rightHandCollider.enabled = false;

         if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

     void PlayPunchSound()
    {
        if (audioSource != null && punchSound != null)
        {
            audioSource.PlayOneShot(punchSound);
        }
    }
    public void EnableLeftHandCollider()
    {
        leftHandCollider.enabled = true;
    }

    public void EnableRightHandCollider()
    {
        rightHandCollider.enabled = true;
    }

    public void DisableLeftHandCollider()
    {
        leftHandCollider.enabled = false;
    }

    public void DisableRightHandCollider()
    {
        rightHandCollider.enabled = false;
    }
    public void EnableDoubleJump(float duration)
    {
        canDoubleJump = true;
        doubleJump = true;
        doubleJumpDuration = duration;
        doubleJumpTimer = duration;

        Invoke(nameof(DisableDoubleJump), duration);
    }

    private void DisableDoubleJump()
    {
        canDoubleJump = false;
    }

private void ProcessInput()
{
    if (Input.GetKeyDown(KeyCode.R) && canUseSpecial)
    {
        PerformSpecialAttack();
    }
}
private void PerformSpecialAttack()
{
    if (Input.GetKeyDown(KeyCode.R) && canUseSpecial)
    {
        Debug.Log("Special Attack Triggered!");
        animator.SetTrigger("BlackFlash"); // Trigger the special attack animation

        
        canUseSpecial = false;
        specialAttackTimer = specialAttackCooldown;

        
        if (audioSource != null && specialAttackSound != null)
        {
            audioSource.PlayOneShot(specialAttackSound);
        }
    }
    else
    {
        Debug.Log("Special attack not ready or key not pressed.");
    }
    animator.ResetTrigger("BlackFlash");
}



   private void UpdateCooldowns()
    {
        if (!canUseSpecial)
        {
            specialAttackTimer -= Time.deltaTime;
            if (specialAttackTimer <= 0)
            {
                canUseSpecial = true;
                specialAttackTimer = 0;
            }
        }

        if (dashCooldownRemaining > 0)
            dashCooldownRemaining -= Time.deltaTime;
    }
void ProcessMovement()
{
   
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");

   
    Vector3 forward = cameraTransform.forward;
    Vector3 right = cameraTransform.right;

   
    forward.y = 0f;
    right.y = 0f;

    
    forward.Normalize();
    right.Normalize();

    
    move = forward * vertical + right * horizontal;


    if (move != Vector3.zero)
    {
        gameObject.transform.forward = move; 
    }

   
    isRunning = Input.GetButton("Run");

   
    float speed = isRunning ? runSpeed : walkSpeed;
    controller.Move(move * Time.deltaTime * speed);

   
    if (Input.GetButtonDown("Jump"))
    {
        if (isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
        else if (canDoubleJump && !doubleJump)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            doubleJump = true;
        }
    }

    
    if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownRemaining <= 0f && !isDashing)
    {
        isDashing = true;
        dashTimeRemaining = dashDuration;
        dashCooldownRemaining = dashCooldown;
    }

    if (isDashing)
    {
        dashTimeRemaining -= Time.deltaTime;
        controller.Move(move * Time.deltaTime * dashSpeed);

        if (dashTimeRemaining <= 0f)
        {
            isDashing = false;
        }
    }

   
    playerVelocity.y += gravity * Time.deltaTime;

   
    controller.Move(playerVelocity * Time.deltaTime);

   
    if (controller.isGrounded)
    {
        playerVelocity.y = -1.0f;
        doubleJump = false;
    }
}


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathPlane"))
        {
            GameManager.Instance.DeincrementScore();
        }

        if (other.CompareTag("YellowPickup"))
        {
            GameManager.Instance.IncrementScore();
            Destroy(other.gameObject, 1.0f);
        }

        if (other.CompareTag("GoalPlane"))
        {
            GameManager.Instance.StoreScore();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
  
    }
    

    void OnClick()
    {
        //  Debug.Log("Punch triggered");
        lastClickedTime = Time.time;
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
        noOfClicks++;

        switch (noOfClicks)
        {
            case 1:
                animator.SetTrigger("CrossPunch");
                PlayLeftPunchEffect();
                
                break;
            case 2:
                animator.SetTrigger("Hook");
               PlayRightPunchEffect();
                break;
            case 3:
                animator.SetTrigger("Punching");
                 PlayLeftPunchEffect();
                break;
            case 4:
                animator.SetTrigger("QuadPunch");
                PlayRightPunchEffect();
                noOfClicks = 0; 
                break;
        }
    }

    void PlayLeftPunchEffect()
    {
        if (leftPunchEffect != null)
        {
            leftPunchEffect.Play();
        }
    }

    void PlayRightPunchEffect()
    {
        if (rightPunchEffect != null)
        {
            rightPunchEffect.Play();
        }
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Enemy"))
    //     {
    //        TakeDamage(5);
    //     }
    // }

    public void TakeDamage(int damageAmount)
    {
        GameManager.Instance.currentHealth -= damageAmount;
         if (audioSource != null && damageSound != null)
    {
        audioSource.PlayOneShot(damageSound);
    }
        if (GameManager.Instance.currentHealth <= 0)
        {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene("LoseScene");
        }
    }

    public void Update()
    {
        ProcessInput();
        UpdateCooldowns();
        if (dashCooldownRemaining > 0)
        {
            dashCooldownRemaining -= Time.deltaTime;
        }

        if (animator.applyRootMotion == false)
        {
            ProcessMovement();
        }
        ProcessGravity();

        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButton(0))
            {
                OnClick();
            }
        }
    }

    public void ProcessGravity()
    {
        if (isGrounded)
        {
            if (playerVelocity.y < 0.0f)
            {
                playerVelocity.y = -1.0f;
            }
        }
        else
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }

        controller.Move(playerVelocity * Time.deltaTime);
        isGrounded = controller.isGrounded;
    }

    private void OnAnimatorMove()
    {
        Vector3 velocity = animator.deltaPosition;
        velocity.y = playerVelocity.y * Time.deltaTime;

        controller.Move(velocity);
    }

    public float GetAnimationSpeed()
    {
        if (isRunning && (move != Vector3.zero))
        {
            return 1.0f;
        }
        else if (move != Vector3.zero)
        {
            return 0.5f;
        }
        else
        {
            return 0f;
        }
    }
}
