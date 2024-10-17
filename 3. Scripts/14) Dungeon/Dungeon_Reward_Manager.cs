using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Reward_Manager : SingleTon<Dungeon_Reward_Manager>
{
    #region "Get Reward"

    public void Get_Sweep_Reward(Dungeon_Type dungeon_type)
    {
        int[] high_stage = Stage_Manager.instance.Get_High_Stage();

        int kill_count = 12;

        Stage_Reward_Manager.instance.Set_Current_Stage_Data(high_stage[0]);

        Budget reward_budget = new Budget();

        List<string> reward_equipments = new List<string>();
        double experience_point = 0.0f;

        switch (dungeon_type)
        {
            case Dungeon_Type.Equipment:
                reward_equipments = Get_Reward_Equipment(Mathf.CeilToInt((float)kill_count / 3));
                break;
            case Dungeon_Type.Experience_Point:
                experience_point = (Stage_Reward_Manager.instance.Get_Experience_Point(high_stage[1]) * 100 * kill_count);
                break;
            case Dungeon_Type.Beyond_Stone:
                reward_budget.beyond_stone = Get_Beyond_Stone(kill_count, high_stage);
                break;
            case Dungeon_Type.Enhance_Stone:
                reward_budget.enhance_stone = Get_Enhance_Stone(kill_count, high_stage);
                break;
            case Dungeon_Type.Gold:
                reward_budget.gold = (Stage_Reward_Manager.instance.Get_Drop_Gold(high_stage[1]) * 5 * kill_count);
                break;
        }

        Reward_Struct new_reward_struct = new Reward_Struct(reward_budget, reward_equipments.ToArray(), experience_point);
        new_reward_struct.Get_Reward();

        Error_Message.instance.Set_Error_Message("Dungeon_Sweep");
    }

    public void Get_Dungeon_Reward(int kill_count, Dungeon_Type dungeon_type, int boost_amount)
    {
        bool stage_mode = Dungeon_Manager.instance.stage_mode;
        int[] high_stage = stage_mode ? Stage_Manager.instance.Get_Current_Stage() : Stage_Manager.instance.Get_High_Stage();

        Stage_Reward_Manager.instance.Set_Current_Stage_Data(high_stage[0]);

        Budget reward_budget = new Budget();

        List<string> reward_equipments = new List<string>();
        double experience_point = 0.0f;

        switch (dungeon_type)
        {
            case Dungeon_Type.Equipment:
                reward_equipments = Get_Reward_Equipment(Mathf.CeilToInt((float)kill_count / 3) * boost_amount);
                break;
            case Dungeon_Type.Experience_Point:
                experience_point = (Stage_Reward_Manager.instance.Get_Experience_Point(high_stage[1]) * 100 * kill_count) * boost_amount;
                break;
            case Dungeon_Type.Beyond_Stone:
                reward_budget.beyond_stone = Get_Beyond_Stone(kill_count, high_stage) * boost_amount;
                break;
            case Dungeon_Type.Enhance_Stone:
                reward_budget.enhance_stone = Get_Enhance_Stone(kill_count, high_stage) * boost_amount;
                break;
            case Dungeon_Type.Gold:
                reward_budget.gold = (Stage_Reward_Manager.instance.Get_Drop_Gold(high_stage[1]) * 5 * kill_count) * boost_amount;
                break;
        }

        Reward_Struct new_reward_struct = new Reward_Struct(reward_budget, reward_equipments.ToArray(), experience_point);

        Reward_Pop_Up.instance.Set_Reward_Pop_Up(Reward_State.Dungeon, new_reward_struct, kill_count);
    }

    #endregion

    #region "Get"

    public List<string> Get_Reward_Equipment(int kill_count)
    {
        List<string> reward_equipments = new List<string>();

        int expect_count = Mathf.CeilToInt((float)kill_count / 3);

        int reward_count = expect_count > 8 ? 8 : expect_count;

        if (reward_count <= 0)
        {
            reward_count = 1;
        }

        for (int i = 0; i < reward_count; i++)
        {
            reward_equipments.Add(Stage_Reward_Manager.instance.Get_Equipment(100));
        }

        return reward_equipments;
    }

    public double Get_Beyond_Stone(int kill_count, int[] high_stage)
    {
        double beyond_stone = ((high_stage[0] * 10) + (high_stage[1] * 0.01f)) * kill_count;

        if (beyond_stone <= 0)
        {
            beyond_stone = 50;
        }

        return beyond_stone;
    }

    public double Get_Enhance_Stone(int kill_count, int[] high_stage)
    {
        double enhance_stone = ((high_stage[0] * 15) + (high_stage[1] * 0.01f)) * kill_count;

        if (enhance_stone <= 0)
        {
            enhance_stone = 10;
        }

        return enhance_stone;
    }

    #endregion
}