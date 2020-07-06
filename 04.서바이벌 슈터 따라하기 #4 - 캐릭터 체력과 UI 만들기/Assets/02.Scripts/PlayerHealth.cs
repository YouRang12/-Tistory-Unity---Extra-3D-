using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip; // 사망시 오디오클립
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    private Animator anim;
    private AudioSource playerAudio;
    private PlayerMovement playerMovement;

    private bool isDead;    // 죽음 유무
    private bool damaged;   // 데미지 유무

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();

        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(damaged)
        {
            // 데미지를 입으면 붉은색 화면
            damageImage.color = flashColour;
        }
        else
        {   // 원래의 화면으로 서서히 돌아옴
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    // 데미지에 따른 HP감소와 죽음
    public void TakeDamage (int amount)
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        playerAudio.Play ();

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }

    // 죽는 애니메이션과 사운드 처리
    void Death ()
    {
        isDead = true;

        anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();    

        playerMovement.enabled = false; // 캐릭터 이동x
    }

    // 게임 재실행(첫 번째 씬으로 돌아옴)
    public void RestartLevel ()
    {
        SceneManager.LoadScene (0);
    }
}
