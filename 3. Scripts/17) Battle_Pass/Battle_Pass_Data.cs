using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Pass_Data : MonoBehaviour
{
    public string pass_name;

    private Dictionary<string, Dictionary<string, object>> csv_data = new Dictionary<string, Dictionary<string, object>>();

    private int requirement;

    #region "Initialize"

    public void Initialize_CSV_Data()
    {
        csv_data = CSVReader.Read($"CSV/Battle_Pass_CSV");

        requirement = int.Parse(csv_data[$"{pass_name}_requirement"]["requirement"].ToString());
    }

    #endregion

    #region "Get"

    public int Requirement()
    {
        return requirement;
    }

    public List<Battle_Pass_Struct> Get_Battle_Pass_Structs()
    {
        List<Battle_Pass_Struct> new_battle_pass = new List<Battle_Pass_Struct>();

        for (int i = 0; i < 7; i++)
        {
            string reward_name = csv_data[$"{pass_name}_battle_pass"][$"reward_name_{i}"].ToString();
            double reward_amount = double.Parse(csv_data[$"{pass_name}_battle_pass"][$"reward_amount_{i}"].ToString());

            Battle_Pass_Struct new_struct = new Battle_Pass_Struct(reward_name, reward_amount);
            new_battle_pass.Add(new_struct);
        }

        return new_battle_pass;
    }

    public List<Battle_Pass_Struct> Get_Free_Pass_Structs()
    {
        List<Battle_Pass_Struct> new_free_pass = new List<Battle_Pass_Struct>();

        for (int i = 0; i < 14; i++)
        {
            string reward_name = csv_data[$"{pass_name}_free_pass"][$"reward_name_{i}"].ToString();
            double reward_amount = double.Parse(csv_data[$"{pass_name}_free_pass"][$"reward_amount_{i}"].ToString());

            Battle_Pass_Struct new_struct = new Battle_Pass_Struct(reward_name, reward_amount);
            new_free_pass.Add(new_struct);
        }

        return new_free_pass;
    }

    #endregion
}