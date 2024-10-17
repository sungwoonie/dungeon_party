using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamble_Pop_Up : SingleTon<Gamble_Pop_Up>
{
    private GameObject pop_up;

    private Gamble_Got_Stat_Content[] got_stat_contents;
    private List<Paid_Stat> current_got_stats = new List<Paid_Stat>();

    private bool is_showing;

    private Gamble_Button current_button;

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
        got_stat_contents = GetComponentsInChildren<Gamble_Got_Stat_Content>(true);
        pop_up = transform.GetChild(0).gameObject;
    }

    #endregion

    #region "Set"

    public void Set_Gamble_Pop_Up(List<Paid_Stat> got_stats, Gamble_Button button)
    {
        is_showing = true;

        current_button = button;

        pop_up.SetActive(true);
        current_got_stats = got_stats;
        StartCoroutine(Set_Got_Stat_Contents());
    }

    private IEnumerator Set_Got_Stat_Contents()
    {
        for (int i = 0; i < current_got_stats.Count; i++)
        {
            if (!got_stat_contents[i].gameObject.activeSelf)
            {
                got_stat_contents[i].Set_Get_Stat_Content(current_got_stats[i]);

                yield return new WaitForSeconds(0.01f);
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        is_showing = false;
    }

    #endregion

    #region "Button"

    public void Re_Gamble()
    {
        if (is_showing)
        {
            return;
        }

        if (current_button != null)
        {
            double price = current_button.gamble_content.current_data.price * current_button.gamble_amount;

            if (Budget_Manager.instance.Can_Use_Budget("diamond", price))
            {
                for (int i = 0; i < got_stat_contents.Length; i++)
                {
                    if (got_stat_contents[i].gameObject.activeSelf)
                    {
                        got_stat_contents[i].gameObject.SetActive(false);
                    }
                }

                current_button.Play_Gamble();
            }
            else
            {
                Debug_Manager.Debug_In_Game_Message($"Can't play {current_button.gamble_content.gamble_type} gamble. not enough diamond");
            }
        }
    }

    public void Close_Button()
    {
        if (is_showing)
        {
            return;
        }

        pop_up.SetActive(false);

        for (int i = 0; i < got_stat_contents.Length; i++)
        {
            if (got_stat_contents[i].gameObject.activeSelf)
            {
                got_stat_contents[i].gameObject.SetActive(false);
            }
        }
    }

    #endregion
}