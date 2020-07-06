using UnityEngine;
using System.Collections;

// 총에 해당
public class Gun : MonoBehaviour
{
    // 발사 모드
    public enum FireMode { Auto, Burst, Single };
    public FireMode fireMode;

    public Transform[] projectileSpawn; // 발사 위치
    public Projectile projectile; // 총알
    public float msBetweenShots = 100; // 연사 간격
    public float muzzleVelocity = 35;  // 총알 속도
    public int burstCount; // 버스트 횟수

    public Transform shell; // 탄피
    public Transform shellEjection; // 탄피 생성위치
    MuzzleFlash muzzleflash; // 머즐플래쉬효과
    float nextShotTime;

    bool triggerReleasedSinceLastShot; // 총알발사 가능여부
    int shotsRemainingInBurst; // 남아있는 버스트 횟수

    void Start()
    {
        muzzleflash = GetComponent<MuzzleFlash>();
        shotsRemainingInBurst = burstCount;
    }

    void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            // 버스트 모드
            if (fireMode == FireMode.Burst)
            {
                if (shotsRemainingInBurst == 0)
                {
                    return;
                }
                shotsRemainingInBurst--;
            }
            // 싱글 모드
            else if (fireMode == FireMode.Single)
            {
                if (!triggerReleasedSinceLastShot)
                {
                    return;
                }
            }
            // 총알 생성
            for (int i = 0; i < projectileSpawn.Length; i++)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation);
                newProjectile.SetSpeed(muzzleVelocity);
            }

            Instantiate(shell, shellEjection.position, shellEjection.rotation); // 탄피 생성
            muzzleflash.Activate(); // 머즐플래쉬 효과
        }
    }
    // 마우스 버튼을 누르고 있을 때
    public void OnTriggerHold()
    {
        Shoot();
        triggerReleasedSinceLastShot = false;
    }
    // 마우스 버튼을 땠을 때
    public void OnTriggerRelease()
    {
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstCount;
    }
}