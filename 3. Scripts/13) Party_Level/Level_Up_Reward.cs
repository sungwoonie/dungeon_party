using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class Level_Up_Reward : MonoBehaviour
{
    private Dictionary<string, Dictionary<string, object>> level_data = new Dictionary<string, Dictionary<string, object>>();

    private Level_Data current_level_data;
    private Level_Data ratio_data;
    private Level_Data base_data;


    #region "Unity"

    private void Awake()
    {
        Initialize_CSV();
    }

    #endregion

    #region "Initialize"

    private void Initialize_CSV()
    {
        level_data = CSVReader.Read("CSV/Level_Data_CSV");
        base_data = Set_Base_Data();
        ratio_data = Ratio_Data();

        current_level_data = new Level_Data();
    }

    private Level_Data Ratio_Data()
    {
        var offset_data = new Level_Data();
        offset_data.equipment_rank_chance = new List<float>();

        var target_level_data = level_data["ratio_data"];

        offset_data.requirement_experience_point = double.Parse(target_level_data["requirement_experience_point"].ToString());

        offset_data.reward_gold = float.Parse(target_level_data["reward_gold"].ToString());

        offset_data.equipment_chance = float.Parse(target_level_data["equipment_chance"].ToString());
        offset_data.key_chance = float.Parse(target_level_data["key_chance"].ToString());

        offset_data.reward_diamond = double.Parse(target_level_data["reward_diamond"].ToString());

        offset_data.equipment_rank_chance.Add(float.Parse(target_level_data["D"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_level_data["C"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_level_data["B"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_level_data["A"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_level_data["S"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_level_data["SS"].ToString()));

        return offset_data;
    }

    private Level_Data Set_Base_Data()
    {
        var offset_data = new Level_Data();
        offset_data.equipment_rank_chance = new List<float>();

        var target_level_data = level_data["base_data"];

        offset_data.requirement_experience_point = double.Parse(target_level_data["requirement_experience_point"].ToString());

        offset_data.reward_gold = float.Parse(target_level_data["reward_gold"].ToString());

        offset_data.equipment_chance = float.Parse(target_level_data["equipment_chance"].ToString());
        offset_data.key_chance = float.Parse(target_level_data["key_chance"].ToString());

        offset_data.reward_diamond = double.Parse(target_level_data["reward_diamond"].ToString());

        offset_data.equipment_rank_chance.Add(float.Parse(target_level_data["D"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_level_data["C"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_level_data["B"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_level_data["A"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_level_data["S"].ToString()));
        offset_data.equipment_rank_chance.Add(float.Parse(target_level_data["SS"].ToString()));

        return offset_data;
    }

    #endregion

    #region "Get Level Data"

    public Level_Data Get_Current_Level_Data(int level)
    {
        current_level_data.requirement_experience_point = base_data.requirement_experience_point * Math.Pow(ratio_data.requirement_experience_point, level - 1);

        current_level_data.reward_gold = base_data.reward_gold * Math.Pow(ratio_data.reward_gold, level - 1);

        current_level_data.key_chance = (float)(base_data.key_chance * Math.Pow(ratio_data.key_chance, level - 1));
        return current_level_data;
    }

    #endregion

    #region "Get Reward"

    public Reward_Struct Get_Level_Up_Reward(Level_Data level_data)
    {
        current_level_data = level_data;

        Budget reward_budget = new Budget();
        reward_budget.gold = current_level_data.reward_gold;
        reward_budget.diamond = current_level_data.reward_diamond;
        reward_budget.key = Get_Key();
        reward_budget.ability_stone = 5;

        List<string> reward_equipments = new List<string>();

        string reward_equipment = Get_Equipment();

        if (!string.IsNullOrEmpty(reward_equipment))
        {
            reward_equipments.Add(reward_equipment);
        }

        Reward_Struct new_reward = new(reward_budget, reward_equipments.ToArray(), 0.0f);

        return new_reward;
    }

    public int Get_Key()
    {
        int chance = UnityEngine.Random.Range(0, 100);

        if (current_level_data.key_chance > chance)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public string Get_Equipment()
    {
        float chance = UnityEngine.Random.Range(0.0f, 100.0f);

        if (chance < current_level_data.equipment_chance)
        {
            float rank_chance = UnityEngine.Random.Range(0.0f, 100.0f);

            int random_class = UnityEngine.Random.Range(0, 3);
            int random_type = UnityEngine.Random.Range(0, 4);
            int rank = 0;

            for (int i = current_level_data.equipment_rank_chance.Count - 1; i >= 0; i--)
            {
                if (rank_chance < current_level_data.equipment_rank_chance[i])
                {
                    rank = i;
                    break;
                }
            }

            string equipment_name = $"Equipment_{random_class}_{random_type}_{rank}_0";
            return "Equipment_1_0_0_0";
            //장비 스크립터블 다 만들면 equipment_name 리턴으로 변경
        }
        else
        {
            return string.Empty;
        }
    }

    #endregion
}