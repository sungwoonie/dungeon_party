using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Paid_Detail_Pop_Up : MonoBehaviour
{
    public Localization_Text title_text;
    public Localization_Text description_text;

    public TMP_Text having_count_text;
    public TMP_Text level_text;
    public TMP_Text rank_text;
    public TMP_Text price_text;

    public Image icon_image;

    public GameObject[] grade_icons;

    public GameObject pop_up;

    [HideInInspector] public Slider having_count_fill;

    [HideInInspector] public Paid_Stat_Content current_content;
    [HideInInspector] public Paid_Stat_Manager stat_manager;

    #region "Unity"

    protected virtual void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    public virtual void Initialize_Component()
    {
        having_count_fill = GetComponentInChildren<Slider>(true);
        stat_manager = GetComponentInParent<Paid_Stat_Manager>();
    }

    #endregion

    #region "Set"
    
    public virtual void Set_Detail_Pop_Up(Paid_Stat_Content target_content)
    {
        current_content = target_content;

        Set_Information();
        Set_Stat_Text();
        Set_Localize_Text();

        if (pop_up)
        {
            pop_up.SetActive(true);
        }
    }

    public virtual void Set_Localize_Text()
    {
        //set on override
    }

    public virtual void Set_Information()
    {
        //set on override
    }

    public virtual void Set_Stat_Text()
    {
        //set on override
    }

    #endregion

    #region "Button"

    public virtual void Enhance_Button()
    {
        if (current_content.paid_stat.having_count <= 0)
        {
            //Can't Enhance. don't have target paid stat
            Debug_Manager.Debug_In_Game_Message($"can't enhance {current_content.paid_stat}. don't have stat");
            Error_Message.instance.Set_Error_Message($"Error_Message_Having_Count_Under_1");
            return;
        }

        if (string.IsNullOrEmpty(stat_manager.budget_type))
        {
            //enhance with having count
            Debug_Manager.Debug_In_Game_Message($"enhance {current_content.paid_stat} by having count");
            Having_Count_Enhance();
        }
        else 
        {
            //enhance with budget
            Debug_Manager.Debug_In_Game_Message($"enhance {current_content.paid_stat} by budget");

            Budget_Enhance();
        }
    }

    public virtual void Merge_Button()
    {
        if (current_content.paid_stat.having_count >= 20)
        {
            //can merge
            current_content.paid_stat.Modify_Data("having_count", current_content.paid_stat.having_count - 20);
            current_content.Initialize_Content();

            Set_Information();

            Merge();
        }
        else
        {
            //not enough having count
            Debug_Manager.Debug_In_Game_Message($"can't merge {current_content.paid_stat}. not enough having count for merge");
            Error_Message.instance.Set_Error_Message($"Error_Message_Having_Count_Under_20");
            return;
        }
    }

    public virtual void Equip_Button()
    {

    }

    #endregion

    #region "Merge"

    public virtual void Merge()
    {
        int max_grade = 3;
        Paid_Rank max_rank = Paid_Rank.SS;

        int current_stat_grade = current_content.paid_stat.grade;
        Paid_Rank current_stat_rank = current_content.paid_stat.rank;

        if (current_stat_grade >= max_grade)
        {
            //rank up

            if (current_stat_rank >= max_rank)
            {
                //already ss rank
                Debug_Manager.Debug_In_Game_Message($"can't merge {current_content.paid_stat}. already max rank");
                Error_Message.instance.Set_Error_Message($"Error_Message_Max_Rank");
            }
            else
            {
                //rank up
                Debug_Manager.Debug_In_Game_Message($"rank merge {current_content.paid_stat}.");
                Rank_Merge();
            }
        }
        else
        {
            //grade up
            Debug_Manager.Debug_In_Game_Message($"grade merge {current_content.paid_stat}.");
            Grade_Merge();
        }
    } 

    public virtual void Grade_Merge()
    {
    }

    public virtual void Rank_Merge()
    {
    }

    #endregion

    #region "Enhance"

    public virtual void Enhance()
    {
        current_content.paid_stat.Modify_Data("level", current_content.paid_stat.level + 1);
        current_content.Initialize_Content();

        Set_Information();
        Set_Stat_Text();

        Debug_Manager.Debug_In_Game_Message($"{current_content.paid_stat} enhanced");
    }

    public virtual void Budget_Enhance()
    {
        double price = current_content.paid_stat.price_offset.Get_Price(current_content.paid_stat.level);

        if (Budget_Manager.instance.Can_Use_Budget(stat_manager.budget_type, price))
        {
            Budget_Manager.instance.Use_Budget(stat_manager.budget_type, price);

            Enhance();
        }
        else
        {
            Debug_Manager.Debug_In_Game_Message($"can't enhance {current_content.paid_stat}. not enough {stat_manager.budget_type}");
            Error_Message.instance.Set_Error_Message($"Error_Message_Not_Enough_Budget");
        }
    }

    public virtual void Having_Count_Enhance()
    {
        if (current_content.paid_stat.having_count >= 20)
        {
            current_content.paid_stat.having_count -= 20;

            Enhance();
        }
        else
        {
            //Can't Enhance. not enough budget
            Debug_Manager.Debug_In_Game_Message($"can't enhance {current_content.paid_stat}. not enough {current_content.paid_stat.having_count}");
            Error_Message.instance.Set_Error_Message($"Error_Message_Having_Count_Under_20");
        }
    }

    #endregion
}