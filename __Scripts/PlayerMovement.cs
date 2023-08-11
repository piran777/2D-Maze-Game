using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Animator animator;

    public float walkSpeed;
    public float runSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;

    bool facingRight = true;

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        if(Input.GetKey("left shift"))
        {
            Move(walkSpeed);
        } else
        {
            Move(runSpeed);
        }
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move(float speed)
    {
        if(moveDirection.x > 0 && !facingRight)
        {
            Flip();
        } else if (moveDirection.x < 0 && facingRight)
        {
            Flip();
        }

        animator.SetFloat("Speed", Mathf.Abs(moveDirection.x * speed));

        if(Mathf.Abs(moveDirection.x * speed) == 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveDirection.y * speed));
        }

        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

}
