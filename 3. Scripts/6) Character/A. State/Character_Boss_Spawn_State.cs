using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Boss_Spawn_State : MonoBehaviour, IState
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

        StartCoroutine(Run_State());
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

    private IEnumerator Run_State()
    {
        Vector3 offset_position = character_controller.Get_Offset_Position();

        animator.SetBool("Run", true);

        while (transform.position != offset_position)
        {
            transform.position = Vector3.MoveTowards(transform.position, offset_position, Time.deltaTime * Game_Time.game_time);
            character_controller.Get_Character_Health_Bar().Set_Health_Bar_Position(transform);
            yield return null;
        }

        character_controller.Set_State_Idle();
    }

    #endregion
}
