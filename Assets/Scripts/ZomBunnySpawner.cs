using System;
using System.Collections;
using UnityEngine;

public class ZomBunnySpawner : MonoBehaviour
{
    private const int ZombunnyHealth = 100;
    private const int ZombunnyDamage = 20;

    [Header("Zombunny")]
    [SerializeField] private GameObject zombunnyPrefab;

    [Header("Spawn Area")]
    [SerializeField] private Transform spawnArea;

    public event Action<EnemyHealth> EnemySpawned;

    public void SpawnWave(int count)
    {
        StartCoroutine(SpawnWaveOverTime(count));
    }

    private IEnumerator SpawnWaveOverTime(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnZombunny();

            if (i < count - 1)
                yield return new WaitForSeconds(3f);
        }
    }

    private void SpawnZombunny()
    {
        if (zombunnyPrefab == null)
        {
            Debug.LogError("ZombunnySpawner needs a Zombunny prefab.", this);
            return;
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject zombunny = Instantiate(zombunnyPrefab, spawnPosition, Quaternion.identity);
        EnemyHealth enemyHealth = zombunny.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.SetHealth(ZombunnyHealth);

            EnemyAttack enemyAttack = zombunny.GetComponent<EnemyAttack>();
            if (enemyAttack != null)
                enemyAttack.SetDamage(ZombunnyDamage);

            EnemySpawned?.Invoke(enemyHealth);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (spawnArea == null)
            return transform.position;

        Vector3 center = spawnArea.position;
        Vector3 size = spawnArea.localScale;

        float randomX = UnityEngine.Random.Range(-size.x / 2f, size.x / 2f);
        float randomZ = UnityEngine.Random.Range(-size.z / 2f, size.z / 2f);

        return new Vector3(
            center.x + randomX,
            center.y,
            center.z + randomZ
        );
    }
}
