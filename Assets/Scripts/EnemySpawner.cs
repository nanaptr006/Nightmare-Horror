using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject hellephantPrefab;

    [Header("Spawn Area")]
    [SerializeField] private Transform spawnArea;

    public event Action<EnemyHealth> EnemySpawned;

    public void SpawnWave(int enemiesPerType)
    {
        SpawnEnemies(enemyPrefab, enemiesPerType);
        SpawnEnemies(hellephantPrefab, enemiesPerType);
    }

    private void SpawnEnemies(GameObject prefab, int count)
    {
        if (prefab == null)
        {
            Debug.LogError("EnemySpawner has an empty enemy prefab reference.", this);
            return;
        }

        for (int i = 0; i < count; i++)
        {
            SpawnEnemy(prefab);
        }
    }

    private void SpawnEnemy(GameObject prefab)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject enemy = Instantiate(prefab, spawnPosition, Quaternion.identity);

        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
            EnemySpawned?.Invoke(enemyHealth);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (spawnArea == null)
        {
            Debug.LogWarning("EnemySpawner has no spawn area. Using its own position.", this);
            return transform.position;
        }

        Vector3 center = spawnArea.position;
        Vector3 size = spawnArea.localScale;
        float randomX = UnityEngine.Random.Range(-size.x / 2f, size.x / 2f);
        float randomZ = UnityEngine.Random.Range(-size.z / 2f, size.z / 2f);

        return new Vector3(center.x + randomX, center.y, center.z + randomZ);
    }
}
