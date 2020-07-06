using UnityEngine;
using System.Collections;

public class Crosshairs : MonoBehaviour
{
    public LayerMask targetMask; // 레이어마스크
    public SpriteRenderer dot;
    public Color dotHighlightColour; // 적인식후 색상
    Color originalDotColour; // 원래 색상

    void Start()
    {
        Cursor.visible = false; // 마우스 커서 숨기기
        originalDotColour = dot.color;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * -40 * Time.deltaTime);
    }
    // 적이 있는지 판별
    public void DetectTargets(Ray ray)
    {
        if (Physics.Raycast(ray, 100, targetMask))
        {
            dot.color = dotHighlightColour;
        }
        else
        {
            dot.color = originalDotColour;
        }
    }
}
