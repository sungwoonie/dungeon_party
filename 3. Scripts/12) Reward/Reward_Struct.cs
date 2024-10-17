public struct Reward_Struct
{
    public Budget reward_budget;
    public double experience_point;
    public string[] reward_equipment;

    public Reward_Struct(Budget _reward_budget, string[] _reward_equipment, double _experience_point)
    {
        reward_budget = _reward_budget;
        reward_equipment = _reward_equipment;
        experience_point = _experience_point;
    }

    public void Get_Reward()
    {
        Budget_Manager.instance.Earn_New_Budget(reward_budget);

        if (reward_equipment != null)
        {
            foreach (var equipment in reward_equipment)
            {
                Stat_Manager.instance.equipment_stat_manager.Get_New_Stat(equipment);
            }
        }

        Party_Level_Manager.instance.Get_Experience_Point(experience_point);

        Reward_Particle_Controller.instance.Play_Particle(0, reward_budget.gold);
        Reward_Particle_Controller.instance.Play_Particle(1, reward_budget.diamond);
        Reward_Particle_Controller.instance.Play_Particle(2, reward_equipment.Length);
    }
}