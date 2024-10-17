using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Stat_Manager : MonoBehaviour
{
    private Dictionary<string, Dictionary<string, object>> csv_data = new Dictionary<string, Dictionary<string, object>>();

    #region "Unity"

    private void Awake()
    {
        Initialize_CSV();
    }

    #endregion

    #region "Initialize"

    private void Initialize_CSV()
    {
        csv_data = CSVReader.Read("CSV/Monster_Stat_CSV");
    }

    #endregion

    #region "Get Stat"

    public Monster_Stat Get_Dungeon_Monster_Stat()
    {
        bool stage_mode = Dungeon_Manager.instance.stage_mode;
        int[] current_stage = stage_mode ? Stage_Manager.instance.Get_Current_Stage() : Stage_Manager.instance.Get_High_Stage();

        double[] monster_stat = Get_Monster_Stat(current_stage, false);

        Monster_Stat current_stat = new Monster_Stat(monster_stat[0], monster_stat[1]);
        return current_stat;
    }

    public Monster_Stat Get_Monster_Stat_By_Current_Stage(bool is_boss)
    {
        int[] current_stage = Stage_Manager.instance.Get_Current_Stage();
        
        double[] monster_stat = Get_Monster_Stat(current_stage, is_boss);

        Monster_Stat current_stat = new Monster_Stat(monster_stat[0], monster_stat[1]);
        return current_stat;
    }

    #endregion

    #region "Get From CSV"

    private double[] Get_Monster_Stat(int[] current_stage, bool is_boss)
    {
        double[] monster_stat = { 0, 0 };

        for (int i = 0; i < 2; i++)
        {
            string target_name = is_boss ? "boss" : "normal";
            string offset_name = i == 0 ? "health" : "damage";

            string data_name = $"{target_name}_{offset_name}";
            string ratio_name = $"{offset_name}_power_ratio";

            float power_ratio = float.Parse(csv_data[current_stage[0].ToString()][ratio_name].ToString());
            double offset = double.Parse(csv_data[current_stage[0].ToString()][data_name].ToString());
            monster_stat[i] = Calculate_Ratio(offset, power_ratio, current_stage[1]);
        }

        return monster_stat;
    }

    private double Calculate_Ratio(double offset, float ratio, int stage)
    {
        double result_ratio = 0.0f;

        result_ratio = offset * ((double)ratio * stage);

        return result_ratio;
    }

    #endregion
}