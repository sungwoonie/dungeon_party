using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Combat_State : MonoBehaviour, IState
{
    private Monster_Controller monster_controller;
    private Animator animator;
    private State_Context state_context;

    #region "Handle"

    public void Handle(Transform controller)
    {
        if (monster_controller == null)
        {
            Initialize_Component(controller);
        }

        StartCoroutine(Combat_State());
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component(Transform controller)
    {
        monster_controller = controller.GetComponent<Monster_Controller>();
        animator = monster_controller.Get_Animator();
        state_context = monster_controller.Get_Monster_State_Context();
    }

    #endregion

    #region "State Methoed"

    private IEnumerator Combat_State()
    {
        Debug_Manager.Debug_In_Game_Message($"{monster_controller.name} monster started combat methoed");

        float attack_delay_offset = 1.0f;
        float current_attack_delay = attack_delay_offset;

        while (state_context.Current_State.Equals(this) && monster_controller.Is_Dead() == false && Event_Bus.Get_Current_State() == Game_State.Combat)
        {
            current_attack_delay -= Time.deltaTime * Game_Time.game_time;

            if (current_attack_delay <= 0)
            {
                //attack
                current_attack_delay = attack_delay_offset;
                animator.SetTrigger("Attack");
            }

            yield return null;
        }

        Debug_Manager.Debug_In_Game_Message($"{monster_controller.name} monster ended combat methoed");
    }

    #endregion
}