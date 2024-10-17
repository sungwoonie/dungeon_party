using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Combat_State : MonoBehaviour, IState
{
    private Character_Controller character_controller;
    private State_Context character_context;
    private Animator animator;

    private float current_attack_speed;
    private float attack_delay;
    private float current_attack_delay;
    private int current_attack_count;
    private int special_attack_change_count;

    #region "Handle"

    public void Handle(Transform controller)
    {
        if (character_controller == null)
        {
            Initialize_Component(controller);
        }

        StopAllCoroutines();
        StartCoroutine(Combat_State());
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

    private void Reset_Combat()
    {
        current_attack_delay = 0;
        current_attack_count = 0;
    }

    private void Update()
    {
        if (current_attack_delay < attack_delay)
        {
            current_attack_delay += Time.deltaTime * current_attack_speed * Game_Time.game_time;
        }
    }

    private IEnumerator Combat_State()
    {
        Debug_Manager.Debug_In_Game_Message($"{character_controller.name}'s combat state is started");

        //Reset_Combat();

        Vector3 monster_position = Monster_Spawner.instance.Get_Current_Monster().transform.position;
        float attack_range = (float)character_controller.current_class.Get_Stat(30);

        if (animator.GetBool("Run") == false)
        {
            animator.SetBool("Run", true);
        }

        while (character_context.Current_State.Equals(this))
        {
            if (Chase(monster_position, attack_range) == false)
            {
                character_controller.Set_Ready_To_Combat(true);
                Combat();
            }
            else
            {
            }

            yield return null;
        }

        Debug_Manager.Debug_In_Game_Message($"{character_controller.name}'s combat state is ended");
    }

    private void Combat()
    {
        attack_delay = (float)Stat_Manager.instance.Calculate_Stat(character_controller.current_class.class_type, 5);
        special_attack_change_count = (int)character_controller.current_class.Get_Stat(22);
        current_attack_speed = (float)Stat_Manager.instance.Calculate_Stat(character_controller.current_class.class_type, 1);

        if (current_attack_delay < attack_delay)
        {
            //current_attack_delay += Time.deltaTime * current_attack_speed * Game_Time.game_time;
        }
        else
        {
            if(current_attack_count >=  special_attack_change_count)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    return;
                }

                Special_Attack();
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    return;
                }

                current_attack_count++;
                current_attack_delay = 0.0f;

                Attack();
            }
        }
    }

    #endregion

    #region "Chase"

    private bool Chase(Vector3 target_position, float attack_range)
    {
        float distance = Vector3.Distance(transform.position, target_position);

        if (distance > attack_range)
        {
            transform.position = Vector3.MoveTowards(transform.position, target_position, Time.deltaTime * Game_Time.game_time);
            distance = Vector3.Distance(transform.position, target_position);

            character_controller.Get_Character_Health_Bar().Set_Health_Bar_Position(transform);
            return true;
        }
        else
        {
            animator.SetBool("Run", false);
            return false;
        }
    }

    #endregion

    #region "Attack"

    private void Attack()
    {
        animator.SetTrigger("Attack");

        int random_sound_number = Random.Range(0, 7);
        Audio_Controller.instance.Play_Audio(1, $"Class_{character_controller.current_class.class_type}_Attack_{random_sound_number}");
        
        Debug_Manager.Debug_In_Game_Message($"{character_controller.name} attacked");
    }

    private void Special_Attack()
    {
        animator.SetTrigger("Special_Attack");
        current_attack_count = 0;

        int random_sound_number = Random.Range(0, 5);
        Audio_Controller.instance.Play_Audio(1, $"Class_{character_controller.current_class.class_type}_Special_Attack_{random_sound_number}");

        Debug_Manager.Debug_In_Game_Message($"{character_controller.name} special attacked");
    }

    #endregion
}