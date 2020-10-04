using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHazardHandler : MonoBehaviour
{
    [SerializeField] float combustionAnimTime = 1f;
    private Animator animator;
    private TimeController timeController;
    private PlayerState playerState;
    private PlayerMovement playerMovement;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        timeController = FindObjectOfType<TimeController>();
        playerState = GetComponent<Player>().GetPlayerState();
        playerMovement = GetComponent<PlayerMovement>();
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
                    playerMovement.SetCanMove(true);
                }
                break;
        }
    }
}
