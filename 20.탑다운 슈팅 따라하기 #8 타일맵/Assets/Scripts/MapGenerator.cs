using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab; // 타일 프리팹
    public Vector2 mapSize; // 맵 사이즈

    [Range(0, 1)]
    public float outlinePercent; // 테두리 영역

    void Start()
    {
        GenerateMap();
    }
    // 맵 생성
    public void GenerateMap()
    {
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
    }
}
