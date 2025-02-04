using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    public Image timerBar;  // UI'deki Image bile�eni
    public float totalTime = 20f;  // 20 saniyelik s�re
    private float timeLeft;

    void Start()
    {
        timeLeft = totalTime;
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / totalTime;  // Zaman ilerledik�e bar azal�r
        }
    }
}