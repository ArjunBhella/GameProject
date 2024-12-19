using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Follow_player : MonoBehaviour
{
      public Transform target; // Reference to the player
    public Vector3 offset = new Vector3(0, 5, -10); // Position offset
    public float smoothSpeed = 0.125f; // Smoothing speed

    void LateUpdate()
    {
        // Target position
        Vector3 targetPosition = target.position + offset;

        // Smooth transition
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        // Make the camera look at the player
        transform.LookAt(target);
    }
    
}
