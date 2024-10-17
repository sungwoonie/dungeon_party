using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Idle_State : MonoBehaviour, IState
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

        Idle_State();
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

    private void Idle_State()
    {
        Debug_Manager.Debug_In_Game_Message($"{monster_controller.name} monster started idle methoed");

        animator.SetBool("Run", false);
    }

    #endregion
}
