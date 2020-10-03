using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] float timeScale = 1f;
    [SerializeField] float rewindTimeScale = 2f;
    [SerializeField] [Tooltip("In seconds")] float rewindTriggerTime = 60;
    PlayerHazardHandler playerHazardHandler;
    float timeSinceLastLoop = 0f;
    float decreasingTime = 0f;

    int roundedTime;
    bool isRewinding = false;
    TimerUI timer;
    TimeBody[] timeBodies;

    // Start is called before the first frame update
    void Start()
    {
        playerHazardHandler = FindObjectOfType<PlayerHazardHandler>();
        timer = FindObjectOfType<TimerUI>();
        timeBodies = FindObjectsOfType<TimeBody>();
    }

    // Update is called once per frame
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
        }
        timer.UpdateTimerText(roundedTime);
        if (Mathf.Abs(timeSinceLastLoop - rewindTriggerTime ) <= Time.deltaTime)
        {
            StartCoroutine(playerHazardHandler.HandleHazard(Hazard.combustion));
            //StartRewind();
        }
        else if(decreasingTime < 0)
        {
            StopRewind();
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
        Time.timeScale = rewindTimeScale;  
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
    }


}
