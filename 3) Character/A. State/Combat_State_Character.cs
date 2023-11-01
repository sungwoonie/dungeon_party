using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_State_Character : MonoBehaviour, IState
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

        StartCoroutine(Combat());
    }

    private IEnumerator Combat()
    {
        float current_delay = 0.0f;

        while (character.state_context.Current_State.Equals(this))
        {
            if (current_delay < Character_Stat_Manager.instance.Get_Attack_Delay(character.current_class))
            {
                current_delay += Time.deltaTime;
            }
            else
            {
                current_delay = 0.0f;
                animator.SetTrigger("Attack");
            }

            yield return null;
        }
    }
}
