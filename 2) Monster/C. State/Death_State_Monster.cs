using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death_State_Monster : MonoBehaviour, IState
{
    private Monster_Behaviour monster;
    private Animator animator;

    public void Handle(Transform controller)
    {
        if (!monster)
        {
            monster = GetComponent<Monster_Behaviour>();
            animator = GetComponent<Animator>();
        }

        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        animator.SetTrigger("Death");

        yield return new WaitForFixedUpdate();

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        if (monster.is_boss)
        {
            Stage_Manager.instance.Stage_Up();
        }
        else
        {
            Stage_Gage.instance.Increase_Stage_Gage(100);
            //give monster reward
        }

        Event_Bus.Publish(Event_Type.Ready);
        gameObject.SetActive(false);
    }
}
