using System.Collections;
using UnityEngine;
using System;

public class LivingEntity : MonoBehaviour,IDamageable
{
    public float startingHealth; // 시작 체력
    protected float health; // 현재 체력
    protected bool dead; // 죽었는지 여부

    public event Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
    }
    // 데미지를 입음
    public void TakeHit(float damage, RaycastHit hit)
    {
        TakeDamage(damage);
    }
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !dead)
            Die();
    }
    // 사망
    protected void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
