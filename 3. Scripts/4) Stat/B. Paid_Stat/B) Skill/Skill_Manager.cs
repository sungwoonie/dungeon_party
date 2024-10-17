using UnityEngine;

/// <summary>
/// Skill_Manager는 스킬의 데미지나 효과를 받아오며, 스킬을 사용하는 스크립트 입니다.
/// </summary>
public class Skill_Manager : SingleTon<Skill_Manager>
{
    public Transform[] classes;

    private Object_Pooling skill_pool;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        skill_pool = GetComponent<Object_Pooling>();
    }

    #endregion

    #region "Use Skill"

    public void Use_Skill(Paid_Stat skill)
    {
        if (skill.Get_Stat(11) > 0)
        {
            //buff
            //use skill to buff. OP
            Debug_Manager.Debug_In_Game_Message($"{skill} type is buff");
            Use_Buff_Skill(skill);
        }
        else
        {
            //active
            Debug_Manager.Debug_In_Game_Message($"{skill} type is active");
            Use_Active_Skill(skill);
        }

        Audio_Controller.instance.Play_Audio(1, skill.name);

        Quest_Manager.instance.Increase_Requirement("skill_use", 1);
    }

    private void Use_Buff_Skill(Paid_Stat skill)
    {
        Vector3 buff_skill_position = Vector3.zero;

        for (int i = 0; i < classes.Length; i++)
        {
            buff_skill_position = classes[i].position + new Vector3(0, 1, 0);

            Set_Up_Skill(buff_skill_position, skill, classes[i]);
        }

        Skill_Buff_Manager.instance.Use_New_Buff_Skill(skill);

        Debug_Manager.Debug_In_Game_Message($"{skill} used to buff");
    }

    private void Use_Active_Skill(Paid_Stat skill)
    {
        Vector3 skill_position = Monster_Spawner.instance.Get_Current_Monster().transform.position;

        Set_Up_Skill(skill_position, skill);

        Debug_Manager.Debug_In_Game_Message($"{skill} used to active");
    }

    #endregion

    #region "Set up skill"

    private void Set_Up_Skill(Vector3 set_up_position, Paid_Stat skill_stat, Transform class_parent = null)
    {
        Skill_Behaviour new_skill = skill_pool.Pool().GetComponent<Skill_Behaviour>();

        if (new_skill.Set_Skill_Behaviour(skill_stat))
        {
            new_skill.transform.position = set_up_position;

            if (class_parent != null)
            {
                new_skill.transform.parent = class_parent;
            }
            else
            {
                new_skill.transform.parent = transform;
            }

            new_skill.gameObject.SetActive(true);
        }

        Debug_Manager.Debug_In_Game_Message($"{new_skill} setted");
    }

    #endregion
}