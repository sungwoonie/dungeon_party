using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Paid_Stat_Content : MonoBehaviour, IPointerClickHandler
{
    public Paid_Stat paid_stat;

    public TMP_Text level_text;
    public TMP_Text having_count_text;

    public TMP_Text rank_text;
    public Image icon;
    public GameObject[] grades;

    public Paid_Detail_Pop_Up detail_pop_up;
    public GameObject not_having_object;
    
    private Slider having_count_fill;
    private Notify notify;


    #region "Unity"

    public virtual void Start()
    {
        Initialize_Content();
    }

    #endregion

    #region "On Click"

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        detail_pop_up.Set_Detail_Pop_Up(this);

        if (notify)
        {
            if (notify.Notified())
            {
                Set_Notify(false);
            }
        }
    }

    #endregion

    #region "Initialize"

    public virtual void Initialize_Component()
    {
        having_count_fill = GetComponentInChildren<Slider>();

        //notify = GetComponent<Notify>();

        //
        if (GetComponent<Notify>())
        {
            notify = GetComponent<Notify>();
        }
    }

    public virtual void Initialize_Content()
    {
        if (having_count_fill == null)
        {
            Initialize_Component();
        }

        if (rank_text != null)
        {
            rank_text.text = paid_stat.rank.ToString();
        }

        if (icon != null)
        {
            icon.sprite = paid_stat.stat_icon;
        }

        if (not_having_object != null)
        {
            not_having_object.SetActive(paid_stat.having_count <= 0 && paid_stat.level <= 1);
        }

        level_text.text = "Lv. " + paid_stat.level;

        having_count_text.text = paid_stat.having_count + " / 20";
        having_count_fill.value = (float)paid_stat.having_count / 20;


        if (grades.Length > 0)
        {
            for (int i = 0; i < grades.Length; i++)
            {
                if (i <= paid_stat.grade)
                {
                    grades[i].SetActive(true);
                }
                else
                {
                    grades[i].SetActive(false);
                }
            }
        }
    }

    #endregion

    #region "Notify"

    public void Set_Notify()
    {
        if (notify)
        {
            bool notified = Anti_Cheat_Manager.instance.Get(paid_stat.name + "_Notified", false);
            
            if (notified)
            {
                notify.Set_On_Notify();
            }
        }
    }

    public void Set_Notify(bool notified)
    {
        if (notify)
        {
            if (notified)
            {
                if (notify.Notified())
                {

                }
                else
                {
                    notify.Set_On_Notify();
                }
            }
            else
            {
                notify.Set_Off_Notify(true);
            }

            Anti_Cheat_Manager.instance.Set(paid_stat.name + "_Notified", notified);
        }
    }

    #endregion
}