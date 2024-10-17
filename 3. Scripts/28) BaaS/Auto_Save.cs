using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auto_Save : MonoBehaviour
{
    public float save_delay;
    public float ranking_update_delay;

    public void Start_Auto_Save()
    {
        Debug_Manager.Debug_Server_Message($"Auto Save Start. Save Delay is {save_delay}. Ranking Update Delay is {ranking_update_delay}");
        StartCoroutine(Auto_Save_Coroutine());
        StartCoroutine(Auto_Update_Ranking());
    }

    private IEnumerator Auto_Update_Ranking()
    {
        float current_update_delay = ranking_update_delay;

        while (true)
        {
            while (current_update_delay > 0)
            {
                current_update_delay -= Time.deltaTime;
                yield return null;
            }

            current_update_delay = ranking_update_delay;

            Debug_Manager.Debug_Server_Message($"Start Update Ranking");

            Ranking_Manager.instance.Start_Update_Ranking();

            yield return null;
        }
    }

    private IEnumerator Auto_Save_Coroutine()
    {
        float current_save_delay = save_delay;

        while (true)
        {
            while (current_save_delay > 0)
            {
                current_save_delay -= Time.deltaTime;
                yield return null;
            }

            current_save_delay = save_delay;

            Debug_Manager.Debug_Server_Message($"Start Auto Save All Data");

            Database_Controller.instance.Update_All_Data_To_Server_Asynch();

            yield return null;
        }
    }
}
