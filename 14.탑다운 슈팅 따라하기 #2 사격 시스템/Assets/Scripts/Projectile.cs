using UnityEngine;

// 발사체를 움직임
public class Projectile : MonoBehaviour
{
    float speed = 10;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
