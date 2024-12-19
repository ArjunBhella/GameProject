using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePickup : MonoBehaviour
{
    public ParticleSystem pickupEffect;
    public float rotationSpeed = 100f;
    private Rigidbody rb;
    public float respawnTime = 30f; 
    private bool isCollected = false;


  private  Collider collider;
    private Renderer renderer;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected) 
        {

            CharacterMovement characterMovement = other.GetComponent<CharacterMovement>();
            if (characterMovement != null)
            {
                
               characterMovement.EnableDoubleJump(30f); 
               StartRespawn(); 
             

            }
        }
    }

    private void StartRespawn()
    {
        pickupEffect.Play();
        isCollected = true;
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        //gameObject.SetActive(false); 


        Invoke(nameof(Respawn), respawnTime);
    }

    private void Respawn()
    {
        isCollected = false;
        GetComponent<Collider>().enabled = true;
        GetComponent<Renderer>().enabled = true;

        //gameObject.SetActive(true); 
    }

    private void Start()
    {
    rb = GetComponent<Rigidbody>();
    collider = GetComponent<Collider>();
    renderer = GetComponent<Renderer>();
     pickupEffect = GetComponent<ParticleSystem>();


    }
 void Update()
    {
       transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

}
