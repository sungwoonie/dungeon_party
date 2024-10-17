using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamble_Content : MonoBehaviour
{
    public Gamble_Type gamble_type;

    public Gamble_Data current_data;

    private Gamble_Button[] gamble_buttons;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    private void Start()
    {
        Initialize_Gamble_Data();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        gamble_buttons = GetComponentsInChildren<Gamble_Button>(true);
    }

    private void Initialize_Gamble_Data()
    {
        current_data = Gamble_Data_Controller.instance.Get_Gamble_Data(gamble_type);

        foreach (var gamble_button in gamble_buttons)
        {
            gamble_button.Initialize_Component();
        }
    }

    #endregion

    #region "Set Chance Information"

    public void Set_Chance_Information()
    {
        Gamble_Chance_Information.instance.Set_Chance_Information(current_data.rank_chance, current_data.grade_chance);
    }

    #endregion

    #region "Gamble"

    public void Play_Gamble(int amount, Gamble_Button button)
    {
        List<Paid_Stat> got_stats = new List<Paid_Stat>();

        for (int i = 0; i < amount; i++)
        {
            got_stats.Add(Gamble(gamble_type));
            Quest_Manager.instance.Increase_Requirement("play_gamble", 1);
        }

        Gamble_Pop_Up.instance.Set_Gamble_Pop_Up(got_stats, button);
    }

    private Paid_Stat Gamble(Gamble_Type type)
    {
        switch (type)
        {
            case Gamble_Type.Equipment:
                return Get_Gamble_Equipment();
            case Gamble_Type.Skill:
                return Get_Gamble_Skill();
            case Gamble_Type.Class:
                return Get_Gamble_Class();
            default:
                return null;
        }
    }

    private Paid_Stat Get_Gamble_Equipment()
    {
        Paid_Stat got_stat = null;

        int random_class = Random.Range(0, 3);
        int random_equipment_type = Random.Range(0, 4);
        string random_rank = Get_Random_Rank();
        string random_grade = Get_Random_Grade();

        string random_code = $"Equipment_{random_class}_{random_equipment_type}_{random_rank}_{random_grade}";

        got_stat = Resources.Load<Paid_Stat>($"1. Scriptable_Object/1) Equipment/Class_{random_class}/Equipment_{random_equipment_type}/{random_code}");

        Stat_Manager.instance.equipment_stat_manager.Get_New_Stat(random_code);

        Debug_Manager.Debug_In_Game_Message($"Got {got_stat.name} !!");

        return got_stat;
    }

    private Paid_Stat Get_Gamble_Skill()
    {
        Paid_Stat got_stat = null;

        int random_active = Random.Range(0, 2);
        int random_rank = int.Parse(Get_Random_Rank());
        int random_kind = Random.Range(0, 6);

        random_kind = Get_Skill_Kind(random_active, random_rank, random_kind);

        string random_code = $"Skill_{random_active}_{random_rank}_{random_kind}";

        got_stat = Resources.Load<Paid_Stat>($"1. Scriptable_Object/2) Skill/{random_code}");

        Stat_Manager.instance.skill_stat_manager.Get_New_Stat(random_code);

        Debug_Manager.Debug_In_Game_Message($"Got {got_stat.name} !!");

        return got_stat;
    }

    private Paid_Stat Get_Gamble_Class()
    {
        Paid_Stat got_stat = null;

        int random_class = Random.Range(0, 3);
        int random_rank = int.Parse(Get_Random_Rank());

        int random_kind = Random.Range(0, 2);

        random_kind = Get_Class_Kind(random_class, random_rank, random_kind);

        string random_code = $"Class_{random_class}_{random_rank}_{random_kind}";

        got_stat = Resources.Load<Paid_Stat>($"1. Scriptable_Object/3) Class/{random_code}");

        Stat_Manager.instance.class_stat_manager.Get_New_Stat(random_code);

        Debug_Manager.Debug_In_Game_Message($"Got {got_stat.name} !!");

        return got_stat;
    }

    #endregion

    #region "Get Random"

    private int Get_Skill_Kind(int active, int rank, int current_kind)
    {
        int current = current_kind;

        while (current >= 0)
        {
            Paid_Stat target_stat = Resources.Load<Paid_Stat>($"1. Scriptable_Object/2) Skill/Skill_{active}_{rank}_{current}");

            if (target_stat != null)
            {
                return current;
            }
            else
            {
                current--;
            }
        }

        return 0;
    }

    private int Get_Class_Kind(int class_type, int rank, int current_kind)
    {
        int current = current_kind;

        while (current >= 0)
        {
            Paid_Stat target_stat = Resources.Load<Paid_Stat>($"1. Scriptable_Object/3) Class/Class_{class_type}_{rank}_{current}");

            if (target_stat != null)
            {
                return current;
            }
            else
            {
                current--;
            }
        }

        return 0;
    }

    private string Get_Random_Grade()
    {
        float random_grade_chance = Random.Range(0, 100);

        float current_chance = 0.0f;

        int grade = 0;

        for (int i = current_data.grade_chance.Length - 1; i >= 0; i--)
        {
            if (current_data.grade_chance[i] == 0)
            {
                continue;
            }

            current_chance += current_data.grade_chance[i];

            if (random_grade_chance <= current_chance)
            {
                grade = i;
                break;
            }
        }

        return grade.ToString();
    }

    private string Get_Random_Rank()
    {
        float random_rank_chance = Random.Range(0, 100);

        float current_chance = 0.0f;

        int rank = 0;

        for (int i = current_data.rank_chance.Length - 1; i >= 0; i--)
        {
            if (current_data.rank_chance[i] == 0)
            {
                continue;
            }

            current_chance += current_data.rank_chance[i];

            if (random_rank_chance <= current_chance)
            {
                rank = i;
                break;
            }
        }

        return rank.ToString();
    }

    #endregion
}