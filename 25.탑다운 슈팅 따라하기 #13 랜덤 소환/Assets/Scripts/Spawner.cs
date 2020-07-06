using UnityEngine;
using System;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public Enemy enemy;

    LivingEntity playerEntity;
    Transform playerT; // 플레이어 트랜스폼

    Wave currentWave; 
    int currentWaveNumber; // 현재 웨이브

    int enemiesRemainingToSpawn; // 남은 소환할 적의 수
    int enemiesRemainingAlive; // 살아있는 적의 수
    float nextSpawnTime; // 다음번 소환 시간

    MapGenerator map;
    /*Camping : 게임에서 유리한 위치에 죽치고 않아있는 비매너 플레이*/
    float timeBetweenCampingChecks = 2; // 캠핑 간격 체크
    float campThresholdDistance = 1.5f; // 캠프 최소 한계거리
    float nextCampCheckTime; // 다음 캠핑 검사 예정 시간
    Vector3 campPositionOld; // 이전 캠프 위치
    bool isCamping; // 캠핑 여부

    bool isDisabled; // 사망 여부

    void Start()
    {
        playerEntity = FindObjectOfType<Player>();
        playerT = playerEntity.transform;

        nextCampCheckTime = timeBetweenCampingChecks + Time.time;
        campPositionOld = playerT.position;
        playerEntity.OnDeath += OnPlayerDeath;

        map = FindObjectOfType<MapGenerator>();
        NextWave();
    }

    void Update()
    {
        if (!isDisabled)
        {
            // 캠핑 체크
            if (Time.time > nextCampCheckTime)
            {
                nextCampCheckTime = Time.time + timeBetweenCampingChecks;

                isCamping = (Vector3.Distance(playerT.position, campPositionOld) < campThresholdDistance);
                campPositionOld = playerT.position;
            }
            // 적 소환 
            if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                StartCoroutine(SpawnEnemy());
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1;
        float tileFlashSpeed = 4;

        Transform spawnTile = map.GetRandomOpenTile();
        if (isCamping)
        {
            spawnTile = map.GetTileFromPosition(playerT.position);
        }
        Material tileMat = spawnTile.GetComponent<Renderer>().material;
        Color initialColour = tileMat.color;
        Color flashColour = Color.red;
        float spawnTimer = 0;

        while (spawnTimer < spawnDelay)
        {

            tileMat.color = Color.Lerp(initialColour, flashColour, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));

            spawnTimer += Time.deltaTime;
            yield return null;
        }

        Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
        spawnedEnemy.OnDeath += OnEnemyDeath;
    }

    void OnPlayerDeath()
    {
        isDisabled = true;
    }

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;

        if (enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        currentWaveNumber++;
        print("Wave: " + currentWaveNumber);
        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
    }
    [Serializable]
    public class Wave
    {
        public int enemyCount; // 소환할 적의 수
        public float timeBetweenSpawns; // 소환 딜레이
    }
}
