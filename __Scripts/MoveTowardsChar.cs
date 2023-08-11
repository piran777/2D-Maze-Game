using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsChar : MonoBehaviour
{
    public GameObject mainChar;
    public float speed = 10f;

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector2.MoveTowards(transform.position, mainChar.transform.position, speed * Time.deltaTime);

        transform.right = mainChar.transform.position - transform.position;
    }
}
