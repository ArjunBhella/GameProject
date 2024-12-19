using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class YellowPickup : MonoBehaviour
{
    // Start is called before the first frame update
   
    
   
    public ParticleSystem pickupEffect;
     public float rotationSpeed = 100f;
    private bool isCollected = false; 
    private Rigidbody rb;

    private  Collider collider;
    private Renderer renderer;

    public GameObject Pickup;

     
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected) 
        {
            pickupEffect.Play();
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
            isCollected = true;
            GameManager.Instance.currentHealth = Mathf.Min(GameManager.Instance.currentHealth + 20, GameManager.Instance.maxHealth);
           
        }
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


