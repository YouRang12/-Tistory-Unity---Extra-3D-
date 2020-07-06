using UnityEngine;
using System.Collections;

public class ScoreKeeper : MonoBehaviour
{
    public static int score { get; private set; }
    float lastEnemyKillTime;
    int streakCount;
    float streakExpiryTime = 1;

    void Start()
    {
        Enemy.OnDeathStatic += OnEnemyKilled;
        FindObjectOfType<Player>().OnDeath += OnPlayerDeath;
    }
    // 적 사망
    void OnEnemyKilled()
    {
        if (Time.time < lastEnemyKillTime + streakExpiryTime)
        {
            streakCount++;
        }
        else
        {
            streakCount = 0;
        }
        lastEnemyKillTime = Time.time;

        score += 5 + (int)Mathf.Pow(2, streakCount);
    }
    // 플레이어 사망
    void OnPlayerDeath()
    {
        Enemy.OnDeathStatic -= OnEnemyKilled;
    }
}
