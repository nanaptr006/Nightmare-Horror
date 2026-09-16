using UnityEngine;

public class ZombunnySpawner : MonoBehaviour
{
    [Header("Zombunny")]
    [SerializeField] private GameObject zombunnyPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private int maxZombunnies = 5;
    [SerializeField] private float spawnInterval = 3f;

    [Header("Spawn Area")]
    [SerializeField] private Transform spawnArea;

    private int currentZombunnies = 0;
    private int zombunniesKilled = 0;
    private float spawnTimer = 0f;

    private void Update()
    {
        // If there are less than 5 Zombunnies
        if (currentZombunnies < maxZombunnies)
        {
            spawnTimer += Time.deltaTime;

            // Spawn after 3 seconds
            if (spawnTimer >= spawnInterval)
            {
                SpawnZombunny();
                spawnTimer = 0f;
            }
        }
        else
        {
            // Reset timer when there are already 5
            spawnTimer = 0f;
        }
    }

    private void SpawnZombunny()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();

        GameObject zombunny = Instantiate(zombunnyPrefab, spawnPosition, Quaternion.identity);

        // Count the new Zombunny
        currentZombunnies++;

        // Get EnemyHealth
        EnemyHealth enemyHealth = zombunny.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            // Listen for the death event
            enemyHealth.Died += OnZombunnyDied;
        }
    }

    private void OnZombunnyDied(EnemyHealth enemy)
    {
        // Stop listening to this enemy
        enemy.Died -= OnZombunnyDied;

        // Decrease the number of living Zombunnies
        currentZombunnies--;

        // Prevent negative number
        currentZombunnies = Mathf.Max(currentZombunnies, 0);

        zombunniesKilled++;

        Debug.Log("Zombunnies killed: " + zombunniesKilled);

        // Start counting 3 seconds again
        spawnTimer = 0f;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 center = spawnArea.position;
        Vector3 size = spawnArea.localScale;

        float randomX = Random.Range(
            -size.x / 2f,
            size.x / 2f
        );

        float randomZ = Random.Range(
            -size.z / 2f,
            size.z / 2f
        );

        return new Vector3(
            center.x + randomX,
            center.y,
            center.z + randomZ
        );
    }
}
