using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject target;       // 카메라가 따라갈 타겟
    public float smoothing = 5.0f;  // 
    private Vector3 offset;         // 카메라와 타겟의 간격

    void Start()
    {
        offset = transform.position - target.transform.position;
    }

    void Update()
    {
        Vector3 newCamPos = target.transform.position + offset;

        // Vector3.Lerp(시작지점, 도착지점, 시간) => 선형보간법으로 한 템포 늦게 플레이어를 따라감(부드럽게 추적)
        transform.position = Vector3.Lerp(transform.position, newCamPos, smoothing * Time.deltaTime);
    }
}
