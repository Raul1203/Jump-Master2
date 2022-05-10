using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimerCountdown : MonoBehaviour
{
    public delegate void GameLost();
    public event GameLost GameLostEvent;
    [SerializeField] TMP_Text countdownText;
    float timeRemaning = 90f;
    bool timeIsRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        timeIsRunning = true;  
    }

    // Update is called once per frame
    void Update()
    {
        if(timeIsRunning)
        {
            if (timeRemaning > 0)
                timeRemaning -= Time.deltaTime;
            else
            {
                timeRemaning = 0;
                timeIsRunning = false;
                GameLostEvent.Invoke();
            }
            DisplayTime(timeRemaning);
        }
    }

    void DisplayTime(float time)
    {
        float minutes=Mathf.FloorToInt(time/60);
        float seconds=Mathf.FloorToInt(time%60);
        string text=string.Format("{0:00}:{1:00}",minutes,seconds); 
        countdownText.text = text;
    }

    
}
