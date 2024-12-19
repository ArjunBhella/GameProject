using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
   
    public GameObject enemyPrefab;

    
    public Vector3 spawnAreaMin; 
    public Vector3 spawnAreaMax; 

    // Spawn control
    public int enemyCount = 5; 
    public float spawnInterval = 2.0f; 

    private float spawnTimer = 0f;
    private int spawnedEnemies = 0;

    void Update()
    {
        if (spawnedEnemies < enemyCount)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                SpawnEnemy();
                spawnTimer = 0f;
            }
        }
    }

    void SpawnEnemy()
    {
        
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        float z = Random.Range(spawnAreaMin.z, spawnAreaMax.z);
        Vector3 spawnPosition = new Vector3(x, y, z);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        spawnedEnemies++;
    }

   
}
