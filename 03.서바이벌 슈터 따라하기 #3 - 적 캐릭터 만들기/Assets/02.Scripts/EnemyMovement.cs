using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Transform player;

    private NavMeshAgent nav; 

    void Awake ()
    {
        // Player 태그로 설정된 오브젝트의 트랜스폼 값을 가져옴
        player = GameObject.FindGameObjectWithTag ("Player").transform;

        nav = GetComponent <NavMeshAgent>();
    }

    void Update()
    {
        // SetDestination(목적지) => 길찾기의 목적지를 설정
        nav.SetDestination(player.position);
    }
}
