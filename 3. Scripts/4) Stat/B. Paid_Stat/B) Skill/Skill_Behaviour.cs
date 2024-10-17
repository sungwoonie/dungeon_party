using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Skill_Behaviour : Observer
{
    private Animator animator;

    private Paid_Stat current_skill_stat;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
        Initialize_Observer();
    }

    private void OnEnable()
    {
        if (animator)
        {
            animator.SetFloat("Game_Time", Game_Time.game_time);
        }
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        animator = GetComponent<Animator>();
    }

    private void Initialize_Observer()
    {
        Game_Time.Attach(this);
    }

    #endregion

    #region "Set Skill"

    public bool Set_Skill_Behaviour(Paid_Stat skill)
    {
        current_skill_stat = skill;

        RuntimeAnimatorController target_animator = Get_Animator(current_skill_stat.name);

        if (target_animator)
        {
            animator.runtimeAnimatorController = target_animator;
        }

        return target_animator;
    }

    private RuntimeAnimatorController Get_Animator(string skill_name)
    {
        string resources_path = $"2. Skill_Animator/{skill_name}";

        if (Resources.Load<RuntimeAnimatorController>(resources_path) == null)
        {
            Debug_Manager.Debug_In_Game_Message($"{skill_name} animator is not exist");
            return null;
        }

        RuntimeAnimatorController animator = Resources.Load<RuntimeAnimatorController>(resources_path);

        return animator;
    }

    #endregion

    #region "Animation"

    public void Skill_Animation_End()
    {
        current_skill_stat = null;
        gameObject.SetActive(false);
    }

    #endregion

    #region "Detection"

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (current_skill_stat.Get_Stat(11) <= 0)
            {
                Monster_Controller new_monster = Monster_Spawner.instance.Get_Current_Monster();

                for (int i = 0; i < current_skill_stat.Get_Stat(12); i++)
                {
                    new_monster.Get_Damage(Final_Damage(current_skill_stat.Get_Stat(13)), true);
                    Debug_Manager.Debug_In_Game_Message($"{current_skill_stat.name} skill gave {current_skill_stat.Get_Stat(13)} damage");
                }
            }
        }
    }

    #endregion

    #region "Damage"

    private double Final_Damage(double damage)
    {
        float ratio = (float)Stat_Manager.instance.Calculate_Stat(13);
        return damage + (damage * (double)ratio * 0.01f);
    }

    #endregion

    #region "Observer"

    public override void Notify()
    {
        if (gameObject.activeSelf)
        {
            animator.SetFloat("Game_Time", Game_Time.game_time);
        }
    }

    #endregion
}