using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour
{
    public GameObject flashHolder; // 머즐 플래쉬
    public Sprite[] flashSprites; // 플래쉬 스프라이트
    public SpriteRenderer[] spriteRenderers;

    public float flashTime; // 플래쉬 시간

    void Start()
    {
        Deactivate();
    }
    // 플래시 효과
    public void Activate()
    {
        flashHolder.SetActive(true);

        int flashSpriteIndex = Random.Range(0, flashSprites.Length);
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = flashSprites[flashSpriteIndex];
        }

        Invoke("Deactivate", flashTime);
    }

    void Deactivate()
    {
        flashHolder.SetActive(false);
    }
}
