using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SukunaFingerSpawner : MonoBehaviour
{
     public GameObject pickupPrefab;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnPickup", 2f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnPickup()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-15, 15), 3, Random.Range(-15, 15));
        
      GameObject spawn =   Instantiate(pickupPrefab, spawnPosition, Quaternion.identity);
        Destroy(spawn,5);
    }
}
