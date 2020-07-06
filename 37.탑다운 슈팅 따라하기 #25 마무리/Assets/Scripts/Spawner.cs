using UnityEngine;
using System;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public bool devMode; // 개발자 모드

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

    public event System.Action<int> OnNewWave;

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
            if ((enemiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                StartCoroutine(SpawnEnemy()); 
            }
            // 개발자 모드
            if (devMode)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    StopCoroutine("SpawnEnemy");
                    // 모든 적 삭제
                    foreach (Enemy enemy in FindObjectsOfType<Enemy>())
                    {
                        GameObject.Destroy(enemy.gameObject);
                    }
                    NextWave(); // 다음 웨이브
                }
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
        // 적 생성
        Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity);
        spawnedEnemy.OnDeath += OnEnemyDeath;
        spawnedEnemy.SetCharacteristics(currentWave.moveSpeed, currentWave.hitsToKillPlayer, 
            currentWave.enemyHealth, currentWave.skinColour); // 적 설정
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
    // 플레이어 위치 리셋
    void ResetPlayerPosition()
    {
        playerT.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up * 3;
    }
    // 다음 웨이브
    void NextWave()
    {
        if (currentWaveNumber > 0)
        {
            AudioManager.instance.PlaySound2D("Level Complete");
        }

        currentWaveNumber++;

        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

            if (OnNewWave != null)
            {
                OnNewWave(currentWaveNumber);
            }
            ResetPlayerPosition();
        }
    }
    [Serializable]
    public class Wave
    {
        public bool infinite; // 무한모드
        public int enemyCount; // 소환할 적의 수
        public float timeBetweenSpawns; // 소환 딜레이

        public float moveSpeed; // 적의 이동속도
        public int hitsToKillPlayer; // 피해 데미지
        public float enemyHealth; // 적 체력
        public Color skinColour; // 적 컬러
    }
}
