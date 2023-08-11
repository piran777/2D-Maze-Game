using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;

    public LayerMask enemyLayers;
    public Transform attackPoint;
    public GameObject player;
    public GameObject healer;

    public float attackRange = 0.5f;
    public int attackDamage;

    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    public int maxPlayerHealth;
    private int currentPlayerHealth;

    public HealthBar healthBar;
    public HealthText healthText;
    public string currentChar = "Hero";
    public bool healerUnlocked = false;
    public HealerMovement healerMovement;


    void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
        healthBar.SetStartingHealth(maxPlayerHealth);
    }
    // Update is called once per frame
    void Update()
    {
        healthBar.SetMaxHealth(maxPlayerHealth);
        healthText.Health(currentPlayerHealth, maxPlayerHealth);

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown("space"))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        if (Input.GetKeyDown("x") && healerUnlocked && currentChar != "Healer")
        {
            player.SetActive(false);
            Instantiate(healer, transform.position, Quaternion.identity);
            //healer.SetActive(true);
            currentChar = "Healer";
            healerMovement.UnlockHealer();
        } else if (Input.GetKeyDown("z") && currentChar != "Hero")
        {
            healer.SetActive(false);
            Instantiate(player, transform.position, Quaternion.identity);
            //player.SetActive(true);
            currentChar = "Hero";
        }
    }

    void Attack()
    {
        //Play Attack Animation
        animator.SetTrigger("Attack");

        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage Them
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void GainStats(string enemyType)
    {
        switch(enemyType)
        {
            case "ZombieEnemy":
                maxPlayerHealth += 1;
                attackDamage += 1;
                break;
            case "SkeletonEnemy":
                maxPlayerHealth += 3;
                attackDamage += 3;
                break;
            case "ArcherEnemy":
                maxPlayerHealth += 6;
                attackDamage += 6;
                break;
        }

        healthBar.SetMaxHealth(maxPlayerHealth);
    }

    public void TakeDamage(int damage)
    {
        currentPlayerHealth -= damage;
        healthBar.SetHealth(currentPlayerHealth);

        if (currentPlayerHealth <= 0)
        {
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
        animator.SetTrigger("Dead");
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public void UnlockHealer()
    {
        healerUnlocked = true;
        healer.SetActive(false);
    }
}
