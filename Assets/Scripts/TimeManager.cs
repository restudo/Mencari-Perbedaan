using System;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    public TimeSpan time;
    public float countdownTime = 60f; // Initial countdown time in seconds
    private float currentTime; // Current countdown time

    private void Start()
    {
        currentTime = countdownTime;
        InvokeRepeating("ExecuteCodeEveryTenSeconds", 10f, 10f); // Execute code every 10 seconds
    }

    private void Update()
    {
        // Update countdown time
        currentTime -= Time.deltaTime;

        time = TimeSpan.FromSeconds(currentTime);

        // If countdown time is up, stop the timer and do something
        if (currentTime <= 0f)
        {
            CancelInvoke("ExecuteCodeEveryTenSeconds");
            Debug.Log("Countdown time is up!");
        }
        else
        {
            timerText.text = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
        }
    }

    private void ExecuteCodeEveryTenSeconds()
    {
        // Code to be executed every 10 seconds
        EventHandler.CallChangeImageTransformEvent();
        Debug.Log("Executing code every 10 seconds");
    }
}
