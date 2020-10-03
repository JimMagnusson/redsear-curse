using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHazardHandler : MonoBehaviour
{
    [SerializeField] float combustionAnimTime = 2f;
    private Animator animator;
    private TimeController timeController;
    private bool isHandlingHazard = false;
    private PlayerState playerState;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        timeController = FindObjectOfType<TimeController>();
        playerState = GetComponent<Player>().GetPlayerState();
    }
    public IEnumerator HandleHazard(Hazard hazard)
    {
        if(!isHandlingHazard)
        {
            isHandlingHazard = true;

            switch (hazard)
            {
                case Hazard.combustion:
                    if(playerState != PlayerState.wet)
                    {
                        animator.SetTrigger("isDying");
                        yield return new WaitForSeconds(combustionAnimTime);
                        animator.SetTrigger("isRewinding");
                        yield return new WaitForSeconds(combustionAnimTime);
                        timeController.StartRewind();
                    }
                    else
                    {
                        // Play small flames anim
                    }
                    break;
            }

            isHandlingHazard = false;
        }
    }
}
