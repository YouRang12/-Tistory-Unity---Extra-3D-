using System.Collections;
using UnityEngine;
using System;

public class LivingEntity : MonoBehaviour,IDamageable
{
    public float startingHealth;
    protected float health;
    protected bool dead;

    public event Action OnDeath;

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
        if (OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
