using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_Reward_Manager : SingleTon<Stage_Reward_Manager>
{
    private Dictionary<string, Dictionary<string, object>> stage_data = new Dictionary<string, Dictionary<string, object>>();

    private Stage_Data current_stage_data;

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
        stage_data = CSVReader.Read("CSV/Stage_Data_CSV");
    }

    #endregion

    #region "Set Stage Data"

    public void Set_Current_Stage_Data(int floor)
    {
        var offset_data = new Stage_Data();
        offset_data.equipment_rank_chance = new List<float>();

        var target_floor_data = stage_data[floor.ToString()];

        offset_data.drop_gold_ratio_minimum = float.Parse(target_floor_data["drop_gold_ratio_minimum"].ToString());
        offset_data.drop_gold_ratio_maximum = float.Parse(target_floor_data["drop_gold_ratio_maximum"].ToString());
        offset_data.drop_gold_offset = double.Parse(target_floor_data["drop_gold_offset"].ToString());

        offset_data.equipment_drop_chance = float.Parse(target_floor_data["equipment_drop_chance"].ToString());
        offset_data.key_drop_chance = float.Parse(target_floor_data["key_drop_chance"].ToString());

        offset_data.drop_diamond = double.Parse(target_floor_data["drop_diamond"].ToString());
        offset_data.diamond_drop_chance = float.Parse(target_floor_data["diamond_drop_chance"].ToString());

        offset_data.experience_point = double.Parse(target_floor_data["experience_point"].ToString());

        offset_data.equipment_rank_chance.Add(float.Parse(target_floor_data["D"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_floor_data["C"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_floor_data["B"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_floor_data["A"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_floor_data["S"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_floor_data["SS"].ToString()));

        current_stage_data = offset_data;
    }

    public Stage_Data Get_Current_Stage_Data(int floor)
    {
        var offset_data = new Stage_Data();
        offset_data.equipment_rank_chance = new List<float>();

        var target_floor_data = stage_data[floor.ToString()];

        offset_data.drop_gold_ratio_minimum = float.Parse(target_floor_data["drop_gold_ratio_minimum"].ToString());
        offset_data.drop_gold_ratio_maximum = float.Parse(target_floor_data["drop_gold_ratio_maximum"].ToString());
        offset_data.drop_gold_offset = double.Parse(target_floor_data["drop_gold_offset"].ToString());

        offset_data.equipment_drop_chance = float.Parse(target_floor_data["equipment_drop_chance"].ToString());
        offset_data.key_drop_chance = float.Parse(target_floor_data["key_drop_chance"].ToString());

        offset_data.drop_diamond = double.Parse(target_floor_data["drop_diamond"].ToString());
        offset_data.diamond_drop_chance = float.Parse(target_floor_data["diamond_drop_chance"].ToString());

        offset_data.experience_point = double.Parse(target_floor_data["experience_point"].ToString());

        offset_data.equipment_rank_chance.Add(float.Parse(target_floor_data["D"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_floor_data["C"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_floor_data["B"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_floor_data["A"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_floor_data["S"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_floor_data["SS"].ToString()));

        return offset_data;
    }

    #endregion

    #region "Get Reward"

    public Reward_Struct Get_Reward(int[] stage, float gold_amount, int equipment_amount, int key_amount, float experience_ratio = 1.0f)
    {
        bool already_got = false;

        if (already_got)
        {
            Debug_Manager.Debug_In_Game_Message($"Can't get reward. already got it");
            return new Reward_Struct();
        }
        else
        {
            Set_Current_Stage_Data(stage[0]);

            Budget reward_budget = new Budget();

            reward_budget.gold = Get_Drop_Gold(stage[1]) * gold_amount;
            reward_budget.diamond = Get_Diamond();
            reward_budget.enhance_stone = Get_Enhance_Stone(stage) * gold_amount;

            for (int i = 0; i < key_amount; i++)
            {
                reward_budget.key += Get_Key();
            }

            double experience_point = Get_Experience_Point(stage[1]) * experience_ratio;

            List<string> reward_equipments = new List<string>();

            for (int i = 0; i < equipment_amount; i++)
            {
                string reward_equipment = Get_Equipment();

                if (!string.IsNullOrEmpty(reward_equipment))
                {
                    reward_equipments.Add(reward_equipment);
                }
            }

            Reward_Struct new_reward = new Reward_Struct(reward_budget, reward_equipments.ToArray(), experience_point);

            return new_reward;
        }
    }

    #endregion

    #region "Get"

    public double Get_Experience_Point(int stage)
    {
        double reward_experience_point = 0.0f;

        reward_experience_point = current_stage_data.experience_point * (stage * 1.05f);

        return reward_experience_point;
    }

    public double Get_Drop_Gold(int stage)
    {
        double reward_gold = 0.0f;

        float drop_gold_ratio = UnityEngine.Random.Range(current_stage_data.drop_gold_ratio_minimum, current_stage_data.drop_gold_ratio_maximum);

        reward_gold = current_stage_data.drop_gold_offset * (stage * 1.08f) * drop_gold_ratio;

        return reward_gold;
    }

    public string Get_Equipment(float drop_chance = 0.0f)
    {
        drop_chance = drop_chance > 0 ? drop_chance : current_stage_data.equipment_drop_chance;
        drop_chance += (float)Stat_Manager.instance.Calculate_Stat(42);

        float chance = Random.Range(0.0f, 100.0f);

        if (chance < drop_chance)
        {
            float rank_chance = Random.Range(0.0f, 100.0f);

            int random_class = Random.Range(0, 3);
            int random_type = Random.Range(0, 4);
            int rank = 0;

            float current_chance = 0.0f;

            for (int i = current_stage_data.equipment_rank_chance.Count - 1; i >= 0; i--)
            {
                current_chance += current_stage_data.equipment_rank_chance[i];

                if (rank_chance <= current_chance)
                {
                    rank = i;
                    break;
                }
            }

            string equipment_name = $"Equipment_{random_class}_{random_type}_{rank}_0";
            return equipment_name;
        }
        else
        {
            return string.Empty;
        }
    }

    public int Get_Key()
    {
        int chance = Random.Range(0, 100);

        if (current_stage_data.key_drop_chance + (float)Stat_Manager.instance.Calculate_Stat(42) > chance)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public double Get_Diamond()
    {
        double diamond = current_stage_data.drop_diamond;
        int chance = UnityEngine.Random.Range(0, 100);

        if (current_stage_data.diamond_drop_chance + (float)Stat_Manager.instance.Calculate_Stat(42) > chance)
        {
            return diamond;
        }
        else
        {
            return 0;
        }
    }

    public double Get_Enhance_Stone(int[] stage)
    {
        double enhance_stone = stage[0] * 2;

        int chance = UnityEngine.Random.Range(0, 100);

        if (15 > chance)
        {
            return enhance_stone;
        }
        else
        {
            return 0;
        }
    }

    #endregion

    #region "Offline Reward"

    public Reward_Struct Get_Reward_Struct_For_Offline_Reward(int[] stage, int[] reward_per_seconds)
    {
        Set_Current_Stage_Data(stage[0]);

        Budget reward_budget = new Budget();

        reward_budget.gold = Get_Drop_Gold(stage[1]) * reward_per_seconds[0];
        reward_budget.diamond = Get_Offline_Diamond(reward_per_seconds[1]);
        string[] reward_equipments = Get_Offline_Equipment(reward_per_seconds[2]);
        reward_budget.key = Get_Offline_Key(reward_per_seconds[3]);
        double experience_point = Get_Experience_Point(stage[1]) * reward_per_seconds[4];

        Reward_Struct new_reward = new Reward_Struct(reward_budget, reward_equipments, experience_point);

        return new_reward;
    }

    private double Get_Offline_Diamond(int count)
    {
        double diamond = 0.0f;

        for (int i = 0; i < count; i++)
        {
            if (diamond > 300.0f)
            {
                break;
            }
            else
            {
                diamond += Get_Diamond();
            }
        }

        return diamond;
    }

    private string[] Get_Offline_Equipment(int count)
    {
        List<string> reward_equipments = new List<string>();

        for (int i = 0; i < count; i++)
        {
            string reward_equipment = Get_Equipment();

            if (!string.IsNullOrEmpty(reward_equipment))
            {
                reward_equipments.Add(reward_equipment);
            }
        }

        return reward_equipments.ToArray();
    }

    private double Get_Offline_Key(int count)
    {
        double key = 0.0f;

        for (int i = 0; i < count; i++)
        {
            if (key > 20.0f)
            {
                break;
            }
            else
            {
                key += Get_Key();
            }
        }

        return key;
    }

    #endregion
}