using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stage_Change_Controller : MonoBehaviour
{
    public TMP_Text[] stage_texts;

    private int[] current_stage;
    private int[] high_stage;

    private GameObject pop_up;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        pop_up = transform.GetChild(0).gameObject;
    }

    #endregion

    #region "Set"

    public void Set_Stage_Change()
    {
        if (Dungeon_Manager.instance.In_Dungeon())
        {
            Error_Message.instance.Set_Error_Message("Error_Message_Change_Stage_In_Dungeon");
            return;
        }

        current_stage = Stage_Manager.instance.Get_Current_Stage();
        high_stage = Stage_Manager.instance.Get_High_Stage();

        for (int i = 0; i < stage_texts.Length; i++)
        {
            stage_texts[i].text = current_stage[i].ToString();
        }

        pop_up.SetActive(true);
    }

    #endregion

    #region "Change Stage"

    private IEnumerator Change_Stage()
    {
        pop_up.SetActive(false);

        Event_Bus.Publish(Game_State.Stop);

        yield return StartCoroutine(Fade.instance.Fade_In());

        Map_Scroller.instance.Change_Map(Stage_Manager.instance.Get_Current_Stage()[0] - 1);

        Character_Manager.instance.Set_All_Characters_To_Offset_Position();

        Monster_Spawner.instance.Remove_All_Spawned_Monsters();

        Stage_Gage.instance.Reset_Stage_Gage();

        Stage_Manager.instance.Set_Stage(current_stage[0], current_stage[1]);

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(Fade.instance.Fade_Out());

        Event_Bus.Publish(Game_State.Spawn_Normal);
    }

    #endregion

    #region "Button"

    public void Change_Button()
    {
        if (Character_Manager.instance.Is_All_Dead())
        {
            Error_Message.instance.Set_Error_Message("Error_Message_Stage_Change_All_Dead");
            return;
        }

        int[] stage = Stage_Manager.instance.Get_Current_Stage();

        if (current_stage[0] == stage[0] && current_stage[1] == stage[1])
        {
            Error_Message.instance.Set_Error_Message("Error_Message_Stage_Change_Same_Stage");
        }
        else
        {
            StartCoroutine(Change_Stage());
        }
    }

    public void Up_Button(bool stage)
    {
        int target = System.Convert.ToInt32(stage);

        if (current_stage[target] + 1 > high_stage[target])
        {
            Error_Message.instance.Set_Error_Message("Error_Message_Stage_Change_Over_High_Stage");
        }
        else
        {
            current_stage[target]++;
            stage_texts[target].text = current_stage[target].ToString();
        }
    }

    public void Down_Button(bool stage)
    {
        int target = System.Convert.ToInt32(stage);

        if (current_stage[target] - 1 < 1)
        {
        }
        else
        {
            current_stage[target]--;
            stage_texts[target].text = current_stage[target].ToString();
        }
    }

    #endregion
}
