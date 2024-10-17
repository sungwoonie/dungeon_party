using System;

[System.Serializable]
public class Stat_Offset
{
    public int stat_type;

    //offset
    public float stat_offset;

    //ratio
    public float stat_ratio;

    #region "Get Amount"

    public double Get_Stat(int level)
    {
        if (level <= 0)
        {
            return 0.0f;
        }

        double stat = level == 1 ? stat_offset : (double)stat_offset * Math.Pow(stat_ratio, level - 1);

        if (stat > double.MaxValue)
        {
            stat = double.MaxValue;
        }

        return stat;
    }

    #endregion
}