using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ready_State_Character : MonoBehaviour, IState
{
    private Character_Behaviour character;

    private Animator animator;

    public void Handle(Transform controller)
    {
        if (!character)
        {
            character = GetComponent<Character_Behaviour>();
            animator = GetComponent<Animator>();
        }

        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        animator.SetBool("Run", true);

        yield return new WaitUntil(() => !character.state_context.Current_State.Equals(this));

        animator.SetBool("Run", false);
    }
}
