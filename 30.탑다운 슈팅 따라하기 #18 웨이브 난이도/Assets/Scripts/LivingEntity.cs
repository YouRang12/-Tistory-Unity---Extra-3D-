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
    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeDamage(damage);
    }
    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !dead)
            Die();
    }
    // 사망
    [ContextMenu("Self Destruct")]
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
