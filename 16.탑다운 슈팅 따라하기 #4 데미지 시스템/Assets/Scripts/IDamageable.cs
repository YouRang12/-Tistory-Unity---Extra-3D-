using UnityEngine;

public interface IDamageable
{
    void TakeHit(float damae, RaycastHit hit);
}
