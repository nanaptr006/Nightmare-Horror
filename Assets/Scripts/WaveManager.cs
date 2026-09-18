using System.Collections;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private const int TotalWaves = 4;
    private const int ZombunniesPerWave = 10;
    private const int HellephantsInFinalWave = 5;
    private static WaveManager activeManager;

    [Header("Spawners")]
    [SerializeField] private ZomBunnySpawner zombunnySpawner;
    [SerializeField] private HellephantSpawner hellephantSpawner;

    [Header("Mission")]
    [SerializeField] private MissionComplete missionComplete;
    [SerializeField] private float firstWaveDelay = 0f;

    [Header("Wave UI")]
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private float waveMessageDuration = 5f;

    private int wavesSpawned;
    private int enemiesKilled;
    private int killsThisWave;
    private int enemiesInCurrentWave;
    private float waveTimer;
    private bool missionFinished;

    private void Awake()
    {
        if (activeManager != null && activeManager != this)
        {
            Debug.LogWarning("Duplicate WaveManager found. This instance will be disabled.", this);
            enabled = false;
            return;
        }

        activeManager = this;

        if (zombunnySpawner == null)
            zombunnySpawner = FindAnyObjectByType<ZomBunnySpawner>();

        if (hellephantSpawner == null)
            hellephantSpawner = FindAnyObjectByType<HellephantSpawner>();

        if (zombunnySpawner == null)
            Debug.LogError("WaveManager needs a ZomBunnySpawner in the scene.", this);

        if (hellephantSpawner == null)
            Debug.LogError("WaveManager needs a HellephantSpawner in the scene.", this);

        KillCount.ResetCount();
        waveTimer = firstWaveDelay;

        if (waveText != null)
            waveText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (zombunnySpawner != null)
            zombunnySpawner.EnemySpawned += TrackEnemy;

        if (hellephantSpawner != null)
            hellephantSpawner.EnemySpawned += TrackEnemy;
    }

    private void OnDisable()
    {
        if (zombunnySpawner != null)
            zombunnySpawner.EnemySpawned -= TrackEnemy;

        if (hellephantSpawner != null)
            hellephantSpawner.EnemySpawned -= TrackEnemy;

        if (activeManager == this)
            activeManager = null;

        StopAllCoroutines();
    }

    private void Start()
    {
        if (zombunnySpawner == null && hellephantSpawner == null)
            return;

        if (firstWaveDelay <= 0f)
            SpawnWave();
    }

    private void Update()
    {
        if (missionFinished || wavesSpawned > 0)
            return;

        waveTimer -= Time.deltaTime;
        if (waveTimer > 0f)
            return;

        SpawnWave();
    }

    private void SpawnWave()
    {
        if (wavesSpawned >= TotalWaves)
        {
            return;
        }

        wavesSpawned++;
        killsThisWave = 0;
        enemiesInCurrentWave = wavesSpawned < TotalWaves
            ? ZombunniesPerWave
            : HellephantsInFinalWave;

        ShowWaveMessage();

        if (wavesSpawned < TotalWaves)
        {
            zombunnySpawner?.SpawnWave(ZombunniesPerWave);
        }
        else
        {
            hellephantSpawner?.SpawnWave(HellephantsInFinalWave);
        }
    }

    private void ShowWaveMessage()
    {
        if (waveText == null)
            return;

        waveText.text = string.Format("Wave {0} / {1}", wavesSpawned, TotalWaves);
        waveText.gameObject.SetActive(true);
        StopCoroutine(nameof(HideWaveMessage));
        StartCoroutine(HideWaveMessage());
    }

    private IEnumerator HideWaveMessage()
    {
        yield return new WaitForSeconds(waveMessageDuration);
        waveText.gameObject.SetActive(false);
    }

    private void TrackEnemy(EnemyHealth enemy)
    {
        enemy.Died += OnEnemyDied;
    }

    private void OnEnemyDied(EnemyHealth enemy)
    {
        enemy.Died -= OnEnemyDied;
        enemiesKilled++;
        killsThisWave++;

        int expectedKills = (TotalWaves - 1) * ZombunniesPerWave + HellephantsInFinalWave;
        if (wavesSpawned >= TotalWaves && enemiesKilled >= expectedKills)
        {
            missionFinished = true;
            missionComplete?.Show(KillCount.GetKilledCount());
        }
        else if (killsThisWave >= enemiesInCurrentWave)
        {
            SpawnWave();
        }
    }
}
