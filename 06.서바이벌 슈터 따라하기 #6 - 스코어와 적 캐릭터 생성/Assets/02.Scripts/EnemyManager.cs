using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;


    void Start ()
    {
        // 무한 반복 호출 함수(함수 이름, 함수호출시간, 반복시간)
        InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }

    void Spawn ()
    {
        // 플레이어가 사망시 적 캐릭터 소환x
        if(playerHealth.currentHealth <= 0f)
        {
            return;
        }

        // SpawnPoint를 기준으로 랜덤 위치에서 생성
        int spawnPointIndex = Random.Range (0, spawnPoints.Length);

        Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
