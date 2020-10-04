using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidBody;
    bool facingLeft = true;
    Vector3 lastPosition;
    Animator animator;

    [SerializeField] GameObject body;
    [SerializeField] float walkingSpeed = 5f;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        lastPosition = transform.position;
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        CheckPositionChange();
        lastPosition = transform.position;
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

    void CheckPositionChange()
    {
        float changeInXPos = lastPosition.x - transform.position.x;
        float changeInYPos = lastPosition.y - transform.position.y;

        if (Mathf.Abs(changeInXPos) >= Mathf.Epsilon) 
        {
            animator.SetBool("PlayerRunning", true);
            if(changeInXPos < 0 && facingLeft) // Moving right
            {
                FlipSprite();
                facingLeft = false;
            }
            else if(changeInXPos > 0 && !facingLeft)
            {
                FlipSprite();
                facingLeft = true;
            }
        }

        if(Mathf.Abs(changeInXPos) >= Mathf.Epsilon || Mathf.Abs(changeInYPos) >= Mathf.Epsilon)
        {
            animator.SetBool("PlayerRunning", true);
        }
        else
        {
            animator.SetBool("PlayerRunning", false);
        }
    }

    void FlipSprite()
    {
        Vector3 bodyScale = body.transform.localScale;
        body.transform.localScale = new Vector3(-bodyScale.x, bodyScale.y, bodyScale.z);
    }
}
