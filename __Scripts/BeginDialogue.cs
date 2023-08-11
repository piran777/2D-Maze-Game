using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginDialogue : MonoBehaviour
{
    public Animator animator;
    public GameObject gameObject;

    public PlayerCombat playerCombat;

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetBool("IsOpen", true);
    }

    public void HideButton()
    {
        animator.SetBool("IsOpen", false);
        playerCombat.UnlockHealer();
        //destroy(this);
    }
}
