using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour
{
    public Rigidbody myRigidbody;
    public float forceMin; // 최소힘
    public float forceMax; // 최대힘

    float lifetime = 4; // 생존시간
    float fadetime = 2; // 페이드시간

    void Start()
    {
        float force = Random.Range(forceMin, forceMax);
        myRigidbody.AddForce(transform.right * force);
        myRigidbody.AddTorque(Random.insideUnitSphere * force);

        StartCoroutine(Fade());
    }
    // 페이드 효과
    IEnumerator Fade()
    {
        yield return new WaitForSeconds(lifetime);

        float percent = 0;
        float fadeSpeed = 1 / fadetime;
        Material mat = GetComponent<Renderer>().material;
        Color initialColour = mat.color;

        while (percent < 1)
        {
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(initialColour, Color.clear, percent);
            yield return null;
        }

        Destroy(gameObject);
    }
}
