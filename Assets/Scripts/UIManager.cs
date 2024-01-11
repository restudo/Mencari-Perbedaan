using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float countdownTime = 60f; // Initial countdown time in seconds //TODO: change value from leveldata
    [SerializeField] private float intervalTime = 10f; //TODO: change value from level data
    private float currentTime; // Current countdown time
    private float currentInterval;
    private TimeSpan time;


    [Header("Health")]
    [SerializeField] private Image[] images;
    private int maxHealth = 0;


    [Header("Objective")]
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private int maxObjective = 5; //TODO: change value from level data
    private int objectiveFounded;

    private void OnEnable()
    {
        EventHandler.DecreaseHealth += DecreaseHealth;
        EventHandler.DifferenceClicked += UpdateObjectiveText;
    }

    private void OnDisable()
    {
        EventHandler.DecreaseHealth -= DecreaseHealth;
        EventHandler.DifferenceClicked -= UpdateObjectiveText;
    }

    private void Start()
    {
        currentTime = countdownTime;
        currentInterval = intervalTime;

        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = true;
            maxHealth++;
        }

        objectiveFounded = 0;
        objectiveText.text = objectiveFounded + "/" + maxObjective;
    }

    private void Update()
    {
        // Update countdown time
        currentTime -= Time.deltaTime;
        time = TimeSpan.FromSeconds(currentTime);

        // If countdown time is up, stop the timer and do something
        if (currentTime <= 0f)
        {
            EventHandler.CallResetImageTransformEvent();
            Debug.Log("Countdown time is up!");
        }
        else
        {
            timerText.text = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);

            currentInterval -= Time.deltaTime;
            if (currentInterval <= 0)
            {
                ChangeImageTransform();
                currentInterval = intervalTime;
            }
        }

    }

    private void ChangeImageTransform()
    {
        // Code to be executed every 10 seconds
        EventHandler.CallChangeImageTransformEvent();
        Debug.Log("Executing code every 10 seconds");
    }

    private void DecreaseHealth()
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].enabled)
            {
                images[i].enabled = false;
                maxHealth--;
                break;
            }
        }

        if (maxHealth <= 0)
        {
            Debug.Log("Lose");
        }
    }

    private void UpdateObjectiveText()
    {
        objectiveFounded++;

        objectiveText.text = objectiveFounded + "/" + maxObjective;
    }
}
