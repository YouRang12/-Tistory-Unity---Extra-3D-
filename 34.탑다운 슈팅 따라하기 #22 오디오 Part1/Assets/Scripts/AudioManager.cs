using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    float masterVolumePercent = 0.2f; // 마스터 볼륨
    float sfxVolumePercent = 1;       // 사운드 효과 볼륨
    float musicVolumePercent = 1f;    // 음악 볼륨

    AudioSource[] musicSources;
    int activeMusicSourceIndex;

    public static AudioManager instance; // 싱글턴

    Transform audioListener;
    Transform playerT;

    void Awake()
    {

        instance = this;

        musicSources = new AudioSource[2];
        for (int i = 0; i < 2; i++)
        {
            // 새로운 AudioSource 추가
            GameObject newMusicSource = new GameObject("Music source " + (i + 1));
            musicSources[i] = newMusicSource.AddComponent<AudioSource>();
            newMusicSource.transform.parent = transform;
        }

        audioListener = FindObjectOfType<AudioListener>().transform;
        playerT = FindObjectOfType<Player>().transform;
    }

    void Update()
    {
        if (playerT != null)
        {
            audioListener.position = playerT.position;
        }
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }
    // 사운드 재생
    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            // 정해진 위치에서 클립 발생
            AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * masterVolumePercent);
        }
    }
    // 음악 크로스페이드
    IEnumerator AnimateMusicCrossfade(float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
            yield return null;
        }
    }
}