using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    // General purpose spawner for effects with cleanup
    public static void SpawnEffect(GameObject effectPrefab, Vector3 position, Quaternion rotation, float duration = 2f)
    {
        if (effectPrefab != null)
        {
            GameObject effectInstance = Instantiate(effectPrefab, position, rotation);
            Destroy(effectInstance, duration); // Automatically clean up after duration
        }
    }
}
