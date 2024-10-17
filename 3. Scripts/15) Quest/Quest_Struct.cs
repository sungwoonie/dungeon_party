using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Quest_Struct
{
    public string quest_name;
    public int requirement;
    public double reward;
    public string reward_name;

    public int current_requirement;

    public Quest_Struct(string _repeat_quest_name, int _requirement, double _reward, int _current_requirement, string _reward_name)
    {
        quest_name = _repeat_quest_name;
        requirement = _requirement;
        reward = _reward;
        reward_name = _reward_name;

        current_requirement = _current_requirement;
    }
}