using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_State_Monster : MonoBehaviour, IState
{
    private Monster_Behaviour monster;
    private Animator animator;

    public void Handle(Transform controller)
    {
        if (!monster)
        {
            monster = controller.GetComponent<Monster_Behaviour>();
            animator = GetComponent<Animator>();
        }

        StartCoroutine(Combat());
    }

    private IEnumerator Combat()
    {
        float current_attack_cool_down = 0.0f;
        float attack_cool_down = 2.0f;

        while (monster.state_context.Current_State.Equals(this))
        {
            if (current_attack_cool_down < attack_cool_down)
            {
                current_attack_cool_down += Time.deltaTime;
            }
            else
            {
                current_attack_cool_down = 0.0f;
                animator.SetTrigger("Attack");
            }

            yield return null;
        }
    }
}
