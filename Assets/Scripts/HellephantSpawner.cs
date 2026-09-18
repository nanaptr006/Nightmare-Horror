using System;
using System.Collections;
using UnityEngine;

public class HellephantSpawner : MonoBehaviour
{
    private const int HellephantHealth = 500;
    private const int HellephantDamage = 30;

    [Header("Hellephant")]
    [SerializeField] private GameObject hellephantPrefab;

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
            if (hellephantPrefab == null)
            {
                Debug.LogError("HellephantSpawner needs a Hellephant prefab.", this);
                yield break;
            }

            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject hellephant = Instantiate(hellephantPrefab, spawnPosition, Quaternion.identity);
            EnemyHealth enemyHealth = hellephant.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.SetHealth(HellephantHealth);

                EnemyAttack enemyAttack = hellephant.GetComponent<EnemyAttack>();
                if (enemyAttack != null)
                    enemyAttack.SetDamage(HellephantDamage);

                EnemySpawned?.Invoke(enemyHealth);
            }

            if (i < count - 1)
                yield return new WaitForSeconds(3f);
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

        return new Vector3(center.x + randomX, center.y, center.z + randomZ);
    }
}
