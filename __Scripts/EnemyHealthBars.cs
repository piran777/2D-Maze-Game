using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBars : MonoBehaviour
{

    Vector3 localScale;

    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
    }

    public void ChangeHealthBar(float maxHealth, float currentHealth)
    {
        localScale.x = (currentHealth / maxHealth);
        transform.localScale = localScale;
    }
}
