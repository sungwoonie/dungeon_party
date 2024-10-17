using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Run_State : MonoBehaviour, IState
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

        StartCoroutine(Run_State());
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

    private IEnumerator Run_State()
    {
        Debug_Manager.Debug_In_Game_Message($"{monster_controller.name} monster started run methoed");

        animator.SetBool("Run", true);

        Vector3 combat_position = Monster_Spawner.instance.Get_Combat_Position();

        while (state_context.Current_State.Equals(this) && transform.position != combat_position)
        {
            transform.position = Vector3.MoveTowards(transform.position, combat_position, Time.deltaTime * 1.5f * Game_Time.game_time);
            Monster_Health_Bar.instance.Set_Monster_Health_Position(transform.position);
            yield return null;
        }

        animator.SetBool("Run", false);

        if (Monster_Spawner.instance.Get_Current_Monster() == monster_controller && Event_Bus.Get_Current_State() == Game_State.Run)
        {
            Event_Bus.Publish(Game_State.Combat);
        }

        Debug_Manager.Debug_In_Game_Message($"{monster_controller.name} monster ended run methoed");
    }

    #endregion
}