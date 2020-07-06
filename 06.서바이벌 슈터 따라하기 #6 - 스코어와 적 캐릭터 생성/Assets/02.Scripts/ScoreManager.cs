using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static int score;

    private Text text;  // Score가 표시될 Text

    void Awake ()
    {
        text = GetComponent <Text> ();
        score = 0;
    }

    // Score 업데이트
    void Update ()
    {
        text.text = "Score: " + score;
    }
}
