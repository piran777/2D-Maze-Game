using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Animator animator;

    public LayerMask playerLayer;
    public Transform attackPoint;
    public Transform player;
    private Rigidbody2D rb;

    public float attackRange;
    public int attackDamage;
    public float attackRate = 4f;
    private float nextAttackTime = 0f;
    public float bulletForce = 5f;

    public GameObject playerRef;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public GameObject bulletPrefab;

    public bool ranged = false;
    public bool canDamagePlayer { get; private set;  }

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (Time.time >= nextAttackTime)
        {
            if (canDamagePlayer)
            {
                if (!ranged)
                {
                    Attack();
                    nextAttackTime = Time.time + 1f / attackRate;
                } else
                {
                    
                    Shoot();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
                
            }
        }

    }

    void Attack()
    {
        //Play Attack Animation
        animator.SetTrigger("Attack");

        //Detect player in range of attack
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        //Damage Them
         foreach (Collider2D player in hitPlayer)
         {
             player.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
         }
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, targetMask);

        if(rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector2 directionToTarget = (target.position - attackPoint.transform.position).normalized;

            if (Vector2.Angle(attackPoint.transform.up, directionToTarget) < 360 / 2)
            {
                float distanceToTarget = Vector2.Distance(attackPoint.transform.position, target.position);

                if (!Physics2D.Raycast(attackPoint.transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canDamagePlayer = true;
                }
                else
                {
                    canDamagePlayer = false;
                }
            }
            else
            {
                canDamagePlayer = false;
            }
        }
        else if (canDamagePlayer)
        {
            canDamagePlayer = false;
        }
    }

    void Shoot()
    {
        animator.SetTrigger("Attack");
        StartCoroutine(BulletWait());
    }

    private IEnumerator BulletWait()
    {
        yield return new WaitForSeconds(1f);

        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
