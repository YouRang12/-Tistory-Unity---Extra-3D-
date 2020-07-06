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
    public int projectilesPerMag; // 총알 갯수
    public float reloadTime = 0.3f;

    // 반동
    [Header("Recoil")]
    public Vector2 kickMinMax = new Vector2(0.05f, 0.2f);
    public Vector2 recoilAngleMinMax = new Vector2(3, 5);
    public float recoilMoveSettleTime = 0.1f;
    public float recoilRotationSettleTime = 0.1f;
    // 효과
    [Header("Effects")]
    public Transform shell; // 탄피
    public Transform shellEjection; // 탄피 생성위치
    public AudioClip shootAudio;
    public AudioClip reloadAudio;

    MuzzleFlash muzzleflash; // 머즐플래쉬효과
    float nextShotTime;

    bool triggerReleasedSinceLastShot; // 총알발사 가능여부
    int shotsRemainingInBurst; // 남아있는 버스트 횟수
    int projectilesRemainingInMag;
    bool isReloading;

    Vector3 recoilSmoothDampVelocity;
    float recoilRotSmoothDampVelocity;
    float recoilAngle; // 반동 각도

    void Start()
    {
        muzzleflash = GetComponent<MuzzleFlash>();
        shotsRemainingInBurst = burstCount;
        projectilesRemainingInMag = projectilesPerMag;
    }

    void LateUpdate()
    {
        // 반동 효과(제자리로)
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilMoveSettleTime);
        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, recoilRotationSettleTime);
        transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;

        if (!isReloading && projectilesRemainingInMag == 0)
        {
            Reload();
        }
    }
    // 발사
    void Shoot()
    {
        if (!isReloading && Time.time > nextShotTime && projectilesRemainingInMag > 0)
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
                if (projectilesRemainingInMag == 0)
                {
                    break;
                }
                projectilesRemainingInMag--;
                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                newProjectile.SetSpeed(muzzleVelocity);
            }

            Instantiate(shell, shellEjection.position, shellEjection.rotation); // 탄피 생성
            muzzleflash.Activate(); // 머즐플래쉬 효과
            transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y); // 반동 효과
            recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);

            AudioManager.instance.PlaySound(shootAudio, transform.position); // 사운드 출력
        }
    }
    // 재장전
    public void Reload()
    {
        if (!isReloading && projectilesRemainingInMag != projectilesPerMag)
        {
            StartCoroutine(AnimateReload());
            AudioManager.instance.PlaySound(reloadAudio, transform.position); // 사운드 출력
        }
    }
    IEnumerator AnimateReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(0.2f);

        float reloadSpeed = 1f / reloadTime;
        float percent = 0;
        Vector3 initialRot = transform.localEulerAngles;
        float maxReloadAngle = 30;

        while (percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;

            yield return null;
        }


        isReloading = false;
        projectilesRemainingInMag = projectilesPerMag;
    }
    // 조준점 바라보기
    public void Aim(Vector3 aimPoint)
    {
        if (!isReloading)
        {
            transform.LookAt(aimPoint);
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