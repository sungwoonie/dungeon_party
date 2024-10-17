
public class Beyond_Stat_Content : Upgradable_Stat_Content
{
    #region "Set"

    public override void Set_Max_Text()
    {
        base.Set_Max_Text();

        int current_level = upgradable_stat.level;

        double current_ratio = upgradable_stat.stat_offset.Get_Stat(current_level);

        string current_ratio_string = Text_Change.ToCurrencyString(current_ratio);

        ratio_text.text = $"{ratio_color}{current_ratio_string}%</color>";
    }

    public override void Set_Level_Text()
    {
        base.Set_Level_Text();

        int current_level = upgradable_stat.level;
        int max_level = upgradable_stat_manager.max_level;

        level_text.text = $"{current_level} / {max_level}";
    }

    public override void Set_Ratio_Text()
    {
        base.Set_Ratio_Text();

        int current_level = upgradable_stat.level;

        double current_ratio = upgradable_stat.stat_offset.Get_Stat(current_level);
        double next_ratio = upgradable_stat.stat_offset.Get_Stat(current_level + 1);

        string current_ratio_string = Text_Change.ToCurrencyString(current_ratio);
        string next_ratio_string = Text_Change.ToCurrencyString(next_ratio);

        ratio_text.text = $"{current_ratio_string}% > {ratio_color}{next_ratio_string}%</color>";
    }

    #endregion
}