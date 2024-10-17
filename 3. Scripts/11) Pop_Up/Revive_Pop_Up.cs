using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Revive_Pop_Up : MonoBehaviour
{
    public TMP_Text timer_text;
    public Paid_Stat ad_buff;

    private GameObject pop_up;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    private void OnEnable()
    {
        Initialize_Event_Bus();
    }

    private void OnDisable()
    {
        Quit_Event_Bus();
    }

    #endregion

    #region "Event Bus"

    private void Initialize_Event_Bus()
    {
        Event_Bus.Subscribe_Event(Game_State.All_Dead, On_Revive_Pop_Up);
    }

    private void Quit_Event_Bus()
    {
        Event_Bus.UnSubscribe_Event(Game_State.All_Dead, On_Revive_Pop_Up);
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        pop_up = transform.GetChild(0).gameObject;
    }

    #endregion

    #region "Set"

    public void On_Revive_Pop_Up()
    {
        pop_up.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(Revive_Timer_On());
    }

    #endregion

    #region "Button"

    public void Restart()
    {
        if (Dungeon_Manager.instance.In_Dungeon())
        {
            Dungeon_Manager.instance.Exit_Dungeon();
            Stage_Gage.instance.Set_Before();
        }
        else
        {
            Stage_Gage.instance.Reset_Stage_Gage();
            Character_Manager.instance.Revive_All();

            Event_Bus.Publish(Game_State.Spawn_Normal);
        }

        pop_up.SetActive(false);
    }

    public void AD_Button()
    {
        Reward_AD_Pop_Up.instance.Set_Reward_AD_Pop_Up("AD_Reward_Revive", Revive);
    }

    #endregion

    #region "Revive"

    public void Revive()
    {
        Skill_Buff_Manager.instance.Use_New_Buff_Skill(ad_buff);

        Character_Manager.instance.Revive_All();

        Event_Bus.Publish(Game_State.Combat);

        pop_up.SetActive(false);
    }

    #endregion

    #region "Timer"

    private IEnumerator Revive_Timer_On()
    {
        float timer = 10.0f;
        timer_text.text = timer.ToString();

        while (timer > 0 && pop_up.activeSelf)
        {
            yield return new WaitForSeconds(1.0f);
            timer -= 1;

            timer_text.text = timer.ToString();
        }

        if(pop_up.activeSelf )
        {
            Restart();
        }

        yield return null;
    }

    #endregion
}