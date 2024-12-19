using UnityEngine;
using Cinemachine;

public class CameraLockOn : MonoBehaviour
{
    [Header("Cinemachine References")]
    public CinemachineFreeLook freeLookCamera; // Reference to the Cinemachine FreeLook Camera
    public CinemachineTargetGroup targetGroup; // Target group for dynamic tracking

    [Header("Targets")]
    public Transform player;  // The player's Transform
    public Transform boss;    // The boss's Transform

    [Header("Camera Settings")]
    public float followSmoothTime = 0.3f;       // Smoothing for camera follow
    public Vector3 cameraOffset = new Vector3(0, 2, -6); // Offset for the player's behind position
    private bool isLockedOn = false;            // State of lock-on mode

    private Vector3 currentVelocity;            // For smooth damping

    void Start()
    {
        // Safety checks
        if (freeLookCamera == null)
        {
            Debug.LogError("Cinemachine FreeLook Camera is not assigned!");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned!");
            return;
        }

        // Initialize the target group
        if (targetGroup != null && boss != null)
        {
            targetGroup.AddMember(player, 1f, 2f); // Add player with weight and radius
            targetGroup.AddMember(boss, 1f, 5f);   // Add boss with weight and radius
        }
    }

    void Update()
    {
        HandleLockOnInput();

        if (!isLockedOn)
        {
            SmoothFollowPlayer(); // Default behavior: smoothly follow the player
        }
    }

    void HandleLockOnInput()
    {
        // Press 'L' to toggle lock-on mode
        if (Input.GetKeyDown(KeyCode.L))
        {
            ToggleLockOn();
        }
    }

    void ToggleLockOn()
    {
        isLockedOn = !isLockedOn;

        if (isLockedOn)
        {
            if (targetGroup != null)
            {
                // Lock onto the target group (includes player and boss)
                freeLookCamera.LookAt = targetGroup.transform;
                freeLookCamera.Follow = targetGroup.transform;

                Debug.Log("Lock-On Enabled: Tracking Player and Boss");
            }
            else
            {
                Debug.LogWarning("TargetGroup is not assigned! Cannot enable lock-on.");
            }
        }
        else
        {
            // Reset to smooth follow behavior (player's behind)
            freeLookCamera.LookAt = player;
            freeLookCamera.Follow = player;

            Debug.Log("Lock-On Disabled: Resetting to Player");
        }
    }

    void SmoothFollowPlayer()
    {
        // Calculate the target position (player's position + offset behind)
        Vector3 targetPosition = player.position + player.TransformDirection(cameraOffset);

        // Smoothly move the camera to the target position
        freeLookCamera.transform.position = Vector3.SmoothDamp(
            freeLookCamera.transform.position,
            targetPosition,
            ref currentVelocity,
            followSmoothTime
        );

        // Ensure the camera always looks at the player
        freeLookCamera.LookAt = player;
        freeLookCamera.Follow = player;
    }
}
