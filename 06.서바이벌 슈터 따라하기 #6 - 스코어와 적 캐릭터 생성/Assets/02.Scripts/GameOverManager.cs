using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;

    private  Animator anim;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 플레이어 사망시 GameOver 문구를 보여줌
        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("GameOver");
        }
    }
}
