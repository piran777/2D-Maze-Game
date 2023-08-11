using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerMovement : MonoBehaviour
{

    public Animator animator;

    public float runSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;

    bool facingRight = true;
    public bool healerUnlocked = false;

    // Update is called once per frame
    void Update()
    {
        if(healerUnlocked)
        {
            ProcessInputs();
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey("left shift") && healerUnlocked)
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
        if (moveDirection.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveDirection.x < 0 && facingRight)
        {
            Flip();
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

    public void UnlockHealer()
    {
        healerUnlocked = true;
    }
}
