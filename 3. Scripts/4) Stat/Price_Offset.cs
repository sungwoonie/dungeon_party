using System;

[System.Serializable]
public class Price_Offset
{
    //offset
    public float price_offset;

    //ratio
    public float price_ratio;

    #region "Get Amount"

    public double Get_Price(int level)
    {
        double stat = price_offset * Math.Pow(price_ratio, level);

        if (stat > double.MaxValue)
        {
            stat = double.MaxValue;
        }

        return stat;
    }

    public double Get_Total_Price(int start_level, int last_lavel)
    {
        double a = price_offset * Math.Pow(price_ratio, start_level);
        double r = price_ratio;
        int n = last_lavel - start_level;

        if (r == 1)
        {
            return a * n;
        }

        double total = a * (Math.Pow(r, n) - 1) / (r - 1);

        if (total > double.MaxValue)
        {
            total = double.MaxValue;
        }

        return total;
    }

    #endregion
}