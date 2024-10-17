public struct Battle_Pass_Struct
{
    public string reward_name;
    public double reward_amount;

    public Battle_Pass_Struct(string _reward_name, double _reward_amount)
    {
        reward_name = _reward_name;
        reward_amount = _reward_amount;
    }
}