using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidBody;

    [SerializeField] float walkingSpeed = 5f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector2 movementVector = new Vector2(horizontalInput, verticalInput);
        movementVector.Normalize();
        Vector2 velocity = movementVector * walkingSpeed;
        rigidBody.velocity = velocity;
    }
}
