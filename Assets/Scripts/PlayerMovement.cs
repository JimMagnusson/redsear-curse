using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidBody;
    bool facingLeft = true;
    bool canMove = true;
    Vector3 lastPosition;
    Animator animator;
    TimeController timeController;


    [SerializeField] GameObject body;
    [SerializeField] float walkingSpeed = 5f;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        lastPosition = transform.position;
        animator = GetComponentInChildren<Animator>();
        timeController = FindObjectOfType<TimeController>();
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
        if (canMove)
        {
            bool isRewinding = timeController.IsRewinding();
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            Vector2 movementVector = new Vector2(horizontalInput, verticalInput);
            movementVector.Normalize();
            Vector2 velocity = movementVector * walkingSpeed;
            rigidBody.velocity = velocity;
            if(!isRewinding)
            {
                if (Mathf.Abs(horizontalInput) > 0f || Mathf.Abs(verticalInput) > 0f)
                {
                    animator.SetBool("PlayerRunning", true);
                }
                else
                {
                    animator.SetBool("PlayerRunning", false);
                }
            }
        }
    }

    void CheckPositionChange()
    {
        float changeInXPos = lastPosition.x - transform.position.x;
        bool isRewinding = timeController.IsRewinding();

        if (Mathf.Abs(changeInXPos) >= Mathf.Epsilon) 
        {
            bool isMovingRightAndForward = changeInXPos < 0 && facingLeft && !isRewinding;
            bool isMovingLeftAndRewinding = changeInXPos > 0 && facingLeft && isRewinding;
            bool isMovingLeftAndForward = changeInXPos > 0 && !facingLeft && !isRewinding;
            bool isMovingRightAndRewinding = changeInXPos < 0 && !facingLeft && isRewinding;

            if (isMovingRightAndForward || isMovingLeftAndRewinding)
            {
                FlipSprite();
                facingLeft = false;
            }
            else if (isMovingLeftAndForward || isMovingRightAndRewinding)
            {
                FlipSprite();
                facingLeft = true;
            }
        }

        if(lastPosition != transform.position && isRewinding)
        {
            animator.SetBool("PlayerRunning", true);
        }
        else if(isRewinding)
        {
            Debug.Log("Setting running anim false");
            animator.SetBool("PlayerRunning", false);
        }
    }

    void FlipSprite()
    {
        Vector3 bodyScale = body.transform.localScale;
        body.transform.localScale = new Vector3(-bodyScale.x, bodyScale.y, bodyScale.z);
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
        rigidBody.velocity = Vector3.zero;      // Reset velocity
        animator.SetBool("PlayerRunning", false);
    }
}
