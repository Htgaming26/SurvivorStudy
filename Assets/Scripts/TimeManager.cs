using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI timeText;

    float gameTime = 0f;
    bool isPlaying = true;
    void Start()
    {
       
    }

    void Update()
    {
        if (!isPlaying) return;
        gameTime += Time.deltaTime;

        int minutes = (int)(gameTime / 60);
        int seconds = (int)(gameTime % 60);

        timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
