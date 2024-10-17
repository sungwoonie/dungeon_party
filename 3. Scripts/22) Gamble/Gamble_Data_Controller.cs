using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamble_Data_Controller : SingleTon<Gamble_Data_Controller>
{
    public List<Gamble_Data> gamble_datas;

    private Dictionary<string, Dictionary<string, object>> csv_datas = new Dictionary<string, Dictionary<string, object>>();

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_CSV();
    }

    #endregion

    #region "Initialize"

    private void Initialize_CSV()
    {
        csv_datas = CSVReader.Read("CSV/Gamble_Data_CSV");

        foreach (var csv_data in csv_datas.Values)
        {
            Gamble_Data new_gamble_data = new Gamble_Data();

            new_gamble_data.gamble_type = (Gamble_Type)System.Enum.Parse(typeof(Gamble_Type), csv_data["Gamble_Type"].ToString());
            new_gamble_data.price = double.Parse(csv_data["Price"].ToString());

            float d_chance = float.Parse(csv_data["D"].ToString());
            float c_chance = float.Parse(csv_data["C"].ToString());
            float b_chance = float.Parse(csv_data["B"].ToString());
            float a_chance = float.Parse(csv_data["A"].ToString());
            float s_chance = float.Parse(csv_data["S"].ToString());
            float ss_chance = float.Parse(csv_data["SS"].ToString());

            float[] rank_chance = { d_chance, c_chance, b_chance, a_chance, s_chance, ss_chance };
            new_gamble_data.rank_chance = rank_chance;

            float grade_1_chance = float.Parse(csv_data["1"].ToString());
            float grade_2_chance = float.Parse(csv_data["2"].ToString());
            float grade_3_chance = float.Parse(csv_data["3"].ToString());
            float grade_4_chance = float.Parse(csv_data["4"].ToString());

            float[] grade_chance = { grade_1_chance, grade_2_chance, grade_3_chance, grade_4_chance };
            new_gamble_data.grade_chance = grade_chance;

            gamble_datas.Add(new_gamble_data);
        }
    }

    #endregion

    #region "Get"

    public Gamble_Data Get_Gamble_Data(Gamble_Type gamble_type)
    {
        Gamble_Data new_gamble_data = new Gamble_Data();

        foreach (var gamble_data in gamble_datas)
        {
            if (gamble_data.gamble_type.Equals(gamble_type))
            {
                new_gamble_data = gamble_data;
            }
        }

        return new_gamble_data;
    }

    #endregion
}