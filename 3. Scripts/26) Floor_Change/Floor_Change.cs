using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor_Change : MonoBehaviour
{
    #region "Unity"

    private void OnEnable()
    {
        Initialize_Event_Bus(true);
    }

    private void OnDisable()
    {
        Initialize_Event_Bus(false);
    }

    #endregion

    #region "Floor Change"

    public void Change()
    {
        StartCoroutine(Change_Floor());
    }

    private IEnumerator Change_Floor()
    {
        Event_Bus.Publish(Game_State.Stop);

        yield return StartCoroutine(Fade.instance.Fade_In());

        Map_Scroller.instance.Change_Map(Stage_Manager.instance.Get_Current_Stage()[0] - 1);

        Character_Manager.instance.Set_All_Characters_To_Offset_Position();

        Monster_Spawner.instance.Remove_All_Spawned_Monsters();

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(Fade.instance.Fade_Out());

        Event_Bus.Publish(Game_State.Spawn_Normal);
    }

    #endregion

    #region "Event Bus"

    private void Initialize_Event_Bus(bool is_on)
    {
        if (is_on)
        {
            Event_Bus.Subscribe_Event(Game_State.Floor_Change, Change);
        }
        else
        {
            Event_Bus.UnSubscribe_Event(Game_State.Floor_Change, Change);
        }
    }

    #endregion
}
