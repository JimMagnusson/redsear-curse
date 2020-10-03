using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerState currentState = PlayerState.normal;

    public PlayerState GetPlayerState()
    {
        return currentState;
    }

    public void SetPlayerState(PlayerState state)
    {
        currentState = state;
    }
}
