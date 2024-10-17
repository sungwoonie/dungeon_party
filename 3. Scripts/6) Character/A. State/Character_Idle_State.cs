using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Idle_State : MonoBehaviour, IState
{
    private Character_Controller character_controller;
    private State_Context character_context;
    private Animator animator;

    #region "Handle"

    public void Handle(Transform controller)
    {
        if (character_controller == null)
        {
            Initialize_Component(controller);
        }

        Idle_State();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component(Transform controller)
    {
        character_controller = controller.GetComponent<Character_Controller>();
        character_context = character_controller.Get_Character_State_Context();
        animator = character_controller.Get_Animator();
    }

    #endregion

    #region "State Methoed"

    private void Idle_State()
    {
        Debug_Manager.Debug_In_Game_Message($"{character_controller.name}'s idle state is started");

        if (animator.GetBool("Run"))
        {
            animator.SetBool("Run", false);
        }
    }

    #endregion
}