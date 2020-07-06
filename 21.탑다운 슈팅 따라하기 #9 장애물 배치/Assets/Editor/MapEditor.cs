using UnityEditor;

// 커스텀 에디터(맵 설정)
[CustomEditor (typeof (MapGenerator))]
public class MapEditor : Editor
{
    // 유니티가 인스펙터를 GUI로 그려주는 메소드
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapGenerator map = target as MapGenerator;

        map.GenerateMap();
    }
}
