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
    [Range(0, 1)]
    public float obstaclePercent; // 장애물 영역

    List<Coord> allTileCoords; // 모든 좌표 리스트
    Queue<Coord> shuffledTileCoords; // 셔플된 좌표 리스트

    public int seed = 10;
    Coord mapCentre; // 맵 정중앙

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
        mapCentre = new Coord((int)mapSize.x/2, (int)mapSize.y/2);

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
        bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];

        int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent);
        int currentObstacleCount = 0;

        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;

            if (randomCoord != mapCentre && MapIsFullyAccessible(obstacleMap, currentObstacleCount))
            {
                Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity);
                newObstacle.parent = mapHolder;
            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }
        }
    }
    // 맵 접근 가능 여부(Flood fill 알고리즘)
    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(mapCentre);
        mapFlags[mapCentre.x, mapCentre.y] = true;

        int accessibleTileCount = 1; // 접근 가능 타일

        while (queue.Count > 0){
            Coord tile = queue.Dequeue();

            for (int x = -1; x <= 1; x++){
                for(int y = -1; y <= 1; y++){
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if (x == 0 || y == 0){
                        if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && 
                            neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1)){
                            if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY]){
                                mapFlags[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Coord(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }
        int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
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
        public static bool operator == (Coord c1, Coord c2)
        {
            return c1.x == c2.x && c1.y == c2.y;
        }
        public static bool operator !=(Coord c1, Coord c2)
        {
            return !(c1 == c2);
        }
    }
}
