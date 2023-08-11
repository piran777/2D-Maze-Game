using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;

    public float maxHealth;
    float currentHealth;

    public PlayerCombat player;

    public string enemyType;
    public GameObject enemy;
    public EnemyHealthBars enemyHealthBars;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        //Play Hurt Animation
        animator.SetTrigger("Hurt");
        enemyHealthBars.ChangeHealthBar(maxHealth, currentHealth);

        if (currentHealth <= 0)
        {
            player.GetComponent<PlayerCombat>().GainStats(enemyType);
            Die();
        }
    }

    void Die()
    {
        StartCoroutine(waiter());

        GetComponent<Collider2D>().enabled = false;

        this.enabled = false;
    }

    IEnumerator waiter()
    {
        animator.SetBool("IsDead", true);
        switch (enemyType)
        {
            case "ZombieEnemy":
                yield return new WaitForSeconds(2.5f);
                break;
            case "SkeletonEnemy":
                yield return new WaitForSeconds(2.5f);
                break;
            case "ArcherEnemy":
                yield return new WaitForSeconds(2.5f);
                break;
        }
        
        Destroy(enemy);
    }


}
