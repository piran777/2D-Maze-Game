using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Animator animator;

    public Transform player;
    private Rigidbody2D rb;
    public float movespeed;

    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer { get; private set; }

    private Vector2 movement;
    bool facingRight = true;

    public float attackRange;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if ((Mathf.Abs(player.position.x) > Mathf.Abs(transform.position.x)) && !facingRight)
        {
            Flip();
        }
        else if ((Mathf.Abs(player.position.x) < Mathf.Abs(transform.position.x)) && facingRight)
        {
            Flip();
        }
        direction.Normalize();
        movement = direction;
    }

    void FixedUpdate()
    {
        Vector2 rangeToAttack = transform.position - playerRef.transform.position;
        if (canSeePlayer)
        {
            if(!(Mathf.Sqrt(Mathf.Pow(rangeToAttack.x, 2) + Mathf.Pow(rangeToAttack.y, 2)) <= attackRange))
            {
                moveCharacter(movement);
            }
        } else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    void moveCharacter(Vector2 direction)
    {
        animator.SetBool("IsMoving", true);
        rb.MovePosition((Vector2)transform.position + (direction * movespeed * Time.deltaTime));
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while(true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        if(rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if(Vector2.Angle(transform.up, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if(!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                } else
                {
                    canSeePlayer = false;
                }
            } else
            {
                canSeePlayer = false;
            }
        } else if(canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);

        Vector3 viewAngle01 = DirectionFromAngle(-transform.eulerAngles.z, -angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(-transform.eulerAngles.z, angle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + viewAngle01 * radius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngle02 * radius);

        if(canSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, playerRef.transform.position);
        }
    }

    private Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
