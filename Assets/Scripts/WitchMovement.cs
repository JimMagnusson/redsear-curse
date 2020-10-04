using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchMovement : MonoBehaviour
{
    [SerializeField] GameObject targetGO;
    [SerializeField] float speed = 10f;
    private Vector2 target;
    private Vector2 position;
    private bool isMoving = false;
    private Animator animator;
    private bool isAtTarget = false;
    
    // Start is called before the first frame update
    void Start()
    {
        target = targetGO.transform.position;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target, step);
            animator.SetBool("WitchWalking", true);
        }
        Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
        if(target == myPos)
        {
            if(!isAtTarget)
            {
                animator.SetBool("WitchWalking", false);
                isAtTarget = true;
            }
        }
    }

    public void SetIsMoving(bool isMoving)
    {
        this.isMoving = isMoving;
    }
    public bool IsMoving()
    {
        return isMoving;
    }

    public bool IsAtTarget()
    {
        return isAtTarget;
    }
}
