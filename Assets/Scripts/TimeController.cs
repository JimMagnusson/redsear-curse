using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] float timeScale = 1f;
    [SerializeField] float rewindTimeScale = 2f;
    [SerializeField] [Tooltip("In seconds")] float combustionTriggerTime = 20f;
    [SerializeField] [Tooltip("In seconds")] float freezeTriggerTime = 40f;
    [SerializeField] [Tooltip("In seconds")] float secondCombustionTriggerTime = 55f;
    [SerializeField] [Tooltip("In seconds")] float secondFreezeTriggerTime = 75f;
    [SerializeField] [Tooltip("In seconds")] float thirdCombustionTriggerTime = 100f;
    PlayerHazardHandler playerHazardHandler;
    float timeSinceLastLoop = 0f;
    float decreasingTime = 0f;

    int roundedTime;
    bool isRewinding = false;
    TimerUI timerUI;
    TimeBody[] timeBodies;
    PlayerMovement playerMovement;

    bool isSendingCoroutine = false;
    bool isHazardsActive = true;

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
            rewindTimeScale += Time.deltaTime;         // increases with time
            rewindTimeScale = Mathf.Clamp(rewindTimeScale, 1f, 20f);
            Time.timeScale = rewindTimeScale;
            if (decreasingTime < 0)
            {
                StopRewind();
                rewindTimeScale = timeScale;
            }
        }
        timerUI.UpdateTimerText(roundedTime);
        if(isHazardsActive)
        {
            HandleHazardTrigger(combustionTriggerTime, Hazard.combustion);
            HandleHazardTrigger(freezeTriggerTime, Hazard.freeze);
            HandleHazardTrigger(secondCombustionTriggerTime, Hazard.combustion);
            HandleHazardTrigger(secondFreezeTriggerTime, Hazard.freeze);
            HandleHazardTrigger(thirdCombustionTriggerTime, Hazard.combustion);
        }

        HandleDebugMode();
    }

    private void HandleHazardTrigger(float triggerTime, Hazard hazard)
    {
        if (Mathf.Abs(timeSinceLastLoop - triggerTime) <= Time.deltaTime && !isSendingCoroutine)
        {
            isSendingCoroutine = true;
            StartCoroutine(playerHazardHandler.HandleHazard(hazard));
        }
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
        ToggleRewindUI(true);

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
        ToggleRewindUI(false);
        playerMovement.SetCanMove(true);
    }

    public bool IsRewinding()
    {
        return isRewinding;
    }

    private void ToggleRewindUI(bool isActive)
    {
        timerUI.toggleRewindIcon(isActive);
        timerUI.toggleRewindEffect(isActive);
    }

    public void SetIsSendingCoroutine(bool isSendingCoroutine)
    {
        this.isSendingCoroutine = isSendingCoroutine;
    }
    public float GetDecreasingTime()
    {
        return decreasingTime;
    }
    public float GetTimeSinceLastLoop()
    {
        return timeSinceLastLoop;
    }
    public void SetHazardsActive(bool isActive)
    {
        isHazardsActive = isActive;
    }
}
