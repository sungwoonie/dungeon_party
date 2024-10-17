using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Game_Manager : SingleTon<Game_Manager>
{
    public Image fade;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Audio_Controller.instance.Play_Audio(0, "Main_BGM");

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        StartCoroutine(Initialize_All_Data());
    }

    #endregion

    #region "Initialize Data"

    private IEnumerator Initialize_All_Data()
    {
        //init data
        User_Database initialized_data = Database_Controller.instance.user_database;

        Battle_Pass_Manager.instance.Initialize_Data(initialized_data.bpd);
        Equipped_Stats.instance.Initialize_Data(initialized_data.e);
        Stage_Manager.instance.Initialize_Data(initialized_data.s);
        Budget_Manager.instance.Initialize_Data(initialized_data.bd);
        Stat_Manager.instance.beyond_stat_manager.Initialize_Data(initialized_data.br);
        Party_Level_Manager.instance.Initialize_Data(initialized_data.ld);
        Quest_Manager.instance.Initialize_Data(initialized_data.qd);
        Daily_Reward_Manager.instance.Initialize_Data(initialized_data.dd);
        User_Data.instance.Initialize_Data(initialized_data.ud);

        if (initialized_data.ld.l >= 15)
        {
            Ranking_Board.instance.Set_Lock_Object(false);
            Ranking_Manager.instance.Set_Ranking_Board();
        }

        yield return StartCoroutine(Fade_Out());

        Offline_Reward_Controller.instance.Initialize_Data(initialized_data.ud.ot);

        Event_Bus.Publish(Game_State.Spawn_Normal);
    }

    #endregion

    #region "Fade"

    public IEnumerator Fade_Out()
    {
        float fade_a = 1.0f;
        Color fade_color = Color.white;

        while (fade_a > 0)
        {
            fade_a -= Time.deltaTime;
            fade_color.a = fade_a;

            fade.color = fade_color;

            yield return null;
        }
    }

    #endregion
}