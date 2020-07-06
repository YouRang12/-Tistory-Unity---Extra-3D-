using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f; // 공격 대기시간
    public int attackDamage = 10;           // 공격 데미지


    private Animator anim;
    private GameObject player;
    private PlayerHealth playerHealth;

    private bool playerInRange; // 플레이어와 충돌처리 중인지 유무
    private float timer;    // 타이머용 


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();

        anim = GetComponent <Animator> ();
    }

    void OnTriggerEnter (Collider other)
    {
        // 플레이어와의 충돌처리(만나면)
        if (other.gameObject == player)
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        // 플레이어와의 충돌처리(멀어지면)
        if (other.gameObject == player)
        {
            playerInRange = false;
        }
    }

    // 공격 대기시간에 따른 공격
    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && playerInRange)
        {
            Attack ();
        }

        if(playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }
    }

    // 공격과 데미지 적용
    void Attack ()
    {
        timer = 0f;

        if(playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
    }
}
