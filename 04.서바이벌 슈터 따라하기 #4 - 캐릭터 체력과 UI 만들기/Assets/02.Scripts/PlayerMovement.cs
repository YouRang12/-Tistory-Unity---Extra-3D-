using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;      // 플레이어 이동속도

    private Animator anim;          // 플레이어 애니메이터
    private Rigidbody rigidBody;    // 플레이어 리지드바디
    private Vector3 movement;       // 플레이어 위치
    private float camRayLength = 100.0f; // 카메라 레이캐스트 거리
    int floorMask;  // 레이어

    void Start()
    {
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        floorMask = LayerMask.GetMask("Floor");
    }

    // 키 매핑
    void Update()
    {
        // 키보드의 입력값을 받아와서 세팅
        float h = Input.GetAxisRaw("Horizontal");   // 좌우값
        float v = Input.GetAxisRaw("Vertical");     // 상하값
        
        Move(h, v);
        Turning();
        Anim(h, v);
    }

    // 이동을 처리하는 함수
    private void Move(float h, float v)
    {
        // Vector.set(x, y, z) : 벡터값을 세팅
        movement.Set(h, 0, v); 

        // normallized : 방향을 가지고옴, Time.deltaTime: 기기에 따른 보정값
        movement = movement.normalized * speed * Time.deltaTime;

        // MovePosition : rigidbody를 이용해서 물체를 이동시킴
        // transform.position : 이 스크립트 객체의 포지션
        rigidBody.MovePosition(transform.position + movement);
    }

    // 회전을 처리하는 함수
    private void Turning()
    {
        // 마우스 위치로 Ray를 만듬
        // ScreenPoitToRay : 2d 화면을 클릭했을 때 3d 좌표계로 계산
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 레이캐스팅 => 만들어진 레이를 발사해서 충돌되는 객체가 있는지 판단
        RaycastHit hitInfo;

        // 레이의 시작위치, 레이충돌 반환, 레이 길이, 충돌한 레이어 
        if(Physics.Raycast(ray, out hitInfo, camRayLength, floorMask))
        {
            // 캐릭터의 방향 계산
            Vector3 playerToMouse = hitInfo.point - transform.position;
            playerToMouse.y = 0;

            // 방향으로 회전
            Quaternion rot = Quaternion.LookRotation(playerToMouse);

            // 회전값 적용
            rigidBody.MoveRotation(rot);
        }

        // 디버그 모드에서 레이를 그려줌
        // 시작위치, 방향, 길이, 색깔, 유지시간
        Debug.DrawRay(ray.origin, ray.direction * camRayLength, Color.red, 0.1f);
    }

    // 애니메이션을 처리하는 함수
    private void Anim(float h, float v)
    {
        // h or v가 0이 아니면 걷는 애니메이션을 재생
        bool isWalking = (h != 0 || v != 0);
        anim.SetBool("IsWalking", isWalking);
    }
}
