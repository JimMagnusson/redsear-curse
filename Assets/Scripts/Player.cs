using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerState currentState = PlayerState.normal;
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(currentState == PlayerState.warm)
        {
            Debug.Log("I'm warm");
            // TODO: write to player in canvas
        }
    }

    public PlayerState GetPlayerState()
    {
        return currentState;
    }

    public void SetPlayerState(PlayerState state)
    {
        currentState = state;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Pond"))
        {
            currentState = PlayerState.wet;
            animator.SetBool("PlayerSubmerged", true);

        }
        else if (collision.gameObject.CompareTag("Campfire"))
        {
            currentState = PlayerState.warm;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pond"))
        {
            currentState = PlayerState.normal;
            animator.SetBool("PlayerSubmerged", false);
        }
        else if(collision.gameObject.CompareTag("Campfire"))
        {
            currentState = PlayerState.normal;
        }
    }
}
