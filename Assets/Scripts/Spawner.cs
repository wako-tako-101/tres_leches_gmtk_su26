using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    public GameObject rainPrefab, lightningPrefab;
    public float spawnInterval = 1f;
    public float lightningChance = 0.2f;
    public float fallSpeed = 4f;
    public float minX = -7f, maxX = 7f;

    void Awake() => Instance = this;

    void SpawnItem()
    {
        GameObject prefab = Random.value < lightningChance ? lightningPrefab : rainPrefab;
        Vector3 pos = new Vector3(Random.Range(minX, maxX), 6f, 0);
        GameObject item = Instantiate(prefab, pos, Quaternion.identity);
        item.GetComponent<FallingItem>().fallSpeed = fallSpeed;
    }

    public void StopSpawning() => CancelInvoke(nameof(SpawnItem));

    public void ApplySettings(float newSpawnInterval, float newLightningChance, float newFallSpeed)
    {
        spawnInterval = newSpawnInterval;
        lightningChance = newLightningChance;
        fallSpeed = newFallSpeed;

        CancelInvoke(nameof(SpawnItem));
        InvokeRepeating(nameof(SpawnItem), 0.5f, spawnInterval);
    }

    public void ClearAllFallingItems()
    {
        FallingItem[] activeItems = FindObjectsByType<FallingItem>(FindObjectsSortMode.None);
        foreach (FallingItem item in activeItems)
        {
            Destroy(item.gameObject);
        }
    }
}