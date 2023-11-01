using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ready_State_Monster : MonoBehaviour ,IState
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

        if (monster.is_boss)
        {
            StartCoroutine(Intro());
        }
        else
        {
            StartCoroutine(Move_To_Stay());
        }
    }

    private IEnumerator Move_To_Stay()
    {
        Vector3 stay_position = new Vector3(1.25f, 2.0f, 0);

        animator.SetBool("Run", true);

        while (transform.position != stay_position)
        {
            transform.position = Vector3.MoveTowards(transform.position, stay_position, Time.deltaTime);
            yield return null;
        }

        animator.SetBool("Run", false);

        Event_Bus.Publish(Event_Type.Combat);
    }

    private IEnumerator Intro()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));

        Event_Bus.Publish(Event_Type.Combat);
    }
}
