using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHazardHandler : MonoBehaviour
{
    [SerializeField] float combustionAnimTime = 1f;
    private Animator animator;
    private TimeController timeController;
     //private bool isHandlingHazard = false;
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
        //if(!isHandlingHazard)
        //{
            Debug.Log("HandlingHazard");
            //isHandlingHazard = true;
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
                        Debug.Log("player no get combustion");
                        playerMovement.SetCanMove(true);
                    }
                    break;
            }
            //isHandlingHazard = false;
        //}
    }
}
