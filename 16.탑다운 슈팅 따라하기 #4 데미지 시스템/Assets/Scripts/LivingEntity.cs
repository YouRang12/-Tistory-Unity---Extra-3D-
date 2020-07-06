using System.Collections;
using UnityEngine;

public class LivingEntity : MonoBehaviour,IDamageable
{
    public float startingHealth;
    protected float health;
    protected bool dead;

    protected virtual void Start()
    {
        health = startingHealth;
    }
    // 데미지를 입음
    public void TakeHit(float damage, RaycastHit hit)
    {
        health -= damage;

        if (health <= 0 && !dead)
            Die();
    }
    // 사망
    protected void Die()
    {
        dead = true;
        GameObject.Destroy(gameObject);
    }
}
