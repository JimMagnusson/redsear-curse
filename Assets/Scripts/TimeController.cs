using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] float timeScale = 1f;
    [SerializeField] float rewindTimeScale = 2f;
    [SerializeField] [Tooltip("In seconds")] float rewindTriggerTime = 60;          // TODO: write one for every hazard
    PlayerHazardHandler playerHazardHandler;
    float timeSinceLastLoop = 0f;
    float decreasingTime = 0f;

    int roundedTime;
    bool isRewinding = false;
    TimerUI timerUI;
    TimeBody[] timeBodies;
    PlayerMovement playerMovement;

    void Start()
    {
        playerHazardHandler = FindObjectOfType<PlayerHazardHandler>();
        timerUI = FindObjectOfType<TimerUI>();
        timeBodies = FindObjectsOfType<TimeBody>();
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        if (!isRewinding)
        {
            timeSinceLastLoop += Time.deltaTime;
            roundedTime = (int)timeSinceLastLoop;
        }
        else
        {
            decreasingTime -= Time.deltaTime;
            roundedTime = (int)decreasingTime;
            // TODO: change timeScale exponentially
            rewindTimeScale += Time.deltaTime;           // should increase by 1 every second?
            Time.timeScale = Mathf.Pow(rewindTimeScale, 2);
            if (decreasingTime < 0)
            {
                StopRewind();
            }
        }
        timerUI.UpdateTimerText(roundedTime);
        if (Mathf.Abs(timeSinceLastLoop - rewindTriggerTime ) <= Time.deltaTime)
        {
            StartCoroutine(playerHazardHandler.HandleHazard(Hazard.combustion));
            //StartRewind();
        }

        HandleDebugMode();
    }

    private void HandleDebugMode()
    {
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Time.timeScale = timeScale;
            }
        }
    }

    public void StartRewind()
    {
        isRewinding = true;
        decreasingTime = timeSinceLastLoop;
        timerUI.toggleRewindIcon(true);

        //Time.timeScale = rewindTimeScale

        foreach (TimeBody timeBody in timeBodies)
        {
            timeBody.StartRewindTimeBody();
        }
    }

    public void StopRewind()
    {
        isRewinding = false;
        Time.timeScale = timeScale;
        foreach (TimeBody timeBody in timeBodies)
        {
            timeBody.StopRewindTimeBody();
        }
        timeSinceLastLoop = 0f;
        timerUI.toggleRewindIcon(false);
        playerMovement.SetCanMove(true);
    }

    public bool IsRewinding()
    {
        return isRewinding;
    }

}
