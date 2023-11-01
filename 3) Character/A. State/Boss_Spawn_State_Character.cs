using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Spawn_State_Character : MonoBehaviour, IState
{
    private Animator animator;

    public void Handle(Transform controller)
    {
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }

        animator.SetBool("Run", false);
    }
}
