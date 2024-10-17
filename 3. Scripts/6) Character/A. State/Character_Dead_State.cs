using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Dead_State : MonoBehaviour, IState
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

        StartCoroutine(Dead_State());
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

    private IEnumerator Dead_State()
    {
        Debug_Manager.Debug_In_Game_Message($"{character_controller.name}'s dead state is started");

        animator.SetBool("Death", true);

        character_controller.Set_Ready_To_Combat(false);

        Character_Manager.instance.Character_Dead(character_controller.current_class.class_type);

        while (character_controller.Get_Dead())
        {
            Game_State current_game_state = Event_Bus.Get_Current_State();

            if (current_game_state == Game_State.Run && transform.position.x > -5.0f)
            {
                transform.Translate(Vector3.left * Time.deltaTime * Game_Time.game_time);
            }

            yield return null;
        }
    }

    #endregion
}
