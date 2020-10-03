using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] float timeScale = 1f;
    [SerializeField] float rewindTimeScale = 2f;
    [SerializeField] [Tooltip("In minutes")] int rewindTriggerTime = 2;
    float timeSinceLastLoop = 0f;
    int roundedTime;
    bool isRewinding = false;
    Timer timer;
    TimeBody[] timeBodies;

    // Start is called before the first frame update
    void Start()
    {
        timer = FindObjectOfType<Timer>();
        timeBodies = FindObjectsOfType<TimeBody>();
    }

    // Update is called once per frame
    void Update()
    {
        roundedTime = (int)timeSinceLastLoop;
        if (!isRewinding)
        {
            timeSinceLastLoop += Time.deltaTime;
        }
        else
        {
            timeSinceLastLoop -= Time.deltaTime;
                                                     // TODO: change timeScale exponentially
        }

        timer.UpdateTimerText(roundedTime);

        if(roundedTime/60 == rewindTriggerTime)
        {
            StartRewind();
        }
        else if(roundedTime <= 0)
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
    }


}
