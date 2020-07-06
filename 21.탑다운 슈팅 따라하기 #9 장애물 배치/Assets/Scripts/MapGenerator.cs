using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab; // 타일 프리팹
    public Transform obstaclePrefab; // 장애물 프리팹
    public Vector2 mapSize; // 맵 사이즈

    [Range(0, 1)]
    public float outlinePercent; // 테두리 영역

    List<Coord> allTileCoords; // 모든 좌표 리스트
    Queue<Coord> shuffledTileCoords; // 셔플된 좌표 리스트

    public int seed = 10; 

    void Start()
    {
        GenerateMap();
    }
    // 맵 생성
    public void GenerateMap()
    {
        allTileCoords = new List<Coord>();
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y)); // 좌표 리스트 추가
            }
        }
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(),seed));

        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y <mapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y/2 + 0.5f + y); // 타일 위치 설정
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)); // 타일 생성
                newTile.localScale = Vector3.one * (1 - outlinePercent); // 테두리 영역 설정
                newTile.parent = mapHolder; // 부모 설정
            }
        }
        int obstacleCount = 10;
        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
            Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity);
            newObstacle.parent = mapHolder;
        }
    }
    // Coord를 Vector3로 변환
    Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
    }
    // 랜덤 좌표 반환
    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }
}
