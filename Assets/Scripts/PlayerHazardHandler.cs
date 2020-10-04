using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHazardHandler : MonoBehaviour
{
    [SerializeField] float combustionAnimTime = 1f;
    [SerializeField] float freezeAnimTime = 2f;
    private Animator animator;
    private TimeController timeController;
    private PlayerState playerState;
    private PlayerMovement playerMovement;
    List<PointInTime> warmFreezePointsInTime;
    List<PointInTime> drownedCombusionPointsInTime;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        timeController = FindObjectOfType<TimeController>();
        playerState = GetComponent<Player>().GetPlayerState();
        playerMovement = GetComponent<PlayerMovement>();
        warmFreezePointsInTime = new List<PointInTime>();
        drownedCombusionPointsInTime = new List<PointInTime>();
    }

    private void Update()
    {
        if(timeController.IsRewinding())
        {
            foreach(PointInTime warmFreezePointInTime in warmFreezePointsInTime)
            {
                bool isSameTime = Mathf.Abs(warmFreezePointInTime.time - timeController.GetDecreasingTime()) < Time.deltaTime/2;  // can cause bugs, be wary
                if(warmFreezePointInTime.position == transform.position && isSameTime)
                {
                    animator.SetTrigger("ReverseWarmFreeze"); // ReverseDrownedCombustion
                }
            }

            foreach (PointInTime drownedCombustionPointInTime in drownedCombusionPointsInTime)
            {
                bool isSameTime = Mathf.Abs(drownedCombustionPointInTime.time - timeController.GetDecreasingTime()) < Time.deltaTime / 2;  // can cause bugs, be wary
                if (drownedCombustionPointInTime.position == transform.position && isSameTime)
                {
                    animator.SetTrigger("ReverseDrownedCombustion"); 
                }
            }
        }
    }
    public IEnumerator HandleHazard(Hazard hazard)
    {
        playerState = GetComponent<Player>().GetPlayerState();
        playerMovement.SetCanMove(false);
        switch (hazard)
        {
            case Hazard.combustion:
                if(playerState != PlayerState.wet)
                {
                    animator.SetTrigger("Combustion");
                    yield return new WaitForSeconds(combustionAnimTime);
                    timeController.StartRewind();
                    animator.SetTrigger("ReverseCombustion");
                }
                else
                {
                    animator.SetTrigger("DrownedCombustion");
                    drownedCombusionPointsInTime.Add(new PointInTime(transform.position, timeController.GetTimeSinceLastLoop()));
                    yield return new WaitForSeconds(0.1f);      // Needed for bugfix, otherwise HandleHazard is called twice.
                    playerMovement.SetCanMove(true);
                }
                timeController.SetIsSendingCoroutine(false);
                break;

            case Hazard.freeze:
                if(playerState != PlayerState.warm)
                {
                    animator.SetTrigger("Freeze");
                    yield return new WaitForSeconds(freezeAnimTime);
                    timeController.StartRewind();
                    animator.SetTrigger("ReverseFreeze");
                }
                else
                {
                    animator.SetTrigger("WarmFreeze");
                    warmFreezePointsInTime.Add(new PointInTime(transform.position, timeController.GetTimeSinceLastLoop()));
                    yield return new WaitForSeconds(0.1f);
                    playerMovement.SetCanMove(true);
                }
                timeController.SetIsSendingCoroutine(false);
                break;
        }
    }
}
