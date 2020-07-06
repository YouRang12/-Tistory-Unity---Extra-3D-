using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioClip mainTheme; // 메인테마 사운드
    public AudioClip menuTheme; // 메뉴 사운드

    void Start()
    {
        AudioManager.instance.PlayMusic(menuTheme, 2);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance.PlayMusic(mainTheme, 3);
        }

    }
}