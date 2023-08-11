using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public LayerMask playerLayer;
    public float attackRange;
    public int attackDamage;

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Detect player in range of attack
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(gameObject.transform.position, attackRange, playerLayer);

        //Damage Them
        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
        }
        Destroy(gameObject, 1f);
    }

}
