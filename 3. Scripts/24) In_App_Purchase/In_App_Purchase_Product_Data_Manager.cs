using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class In_App_Purchase_Product_Data_Manager : MonoBehaviour
{
    public List<In_App_Purchase_Product_Data> product_datas = new List<In_App_Purchase_Product_Data>();

    private Dictionary<string, Dictionary<string, object>> csv_datas;

    #region "Unity"

    private void Awake()
    {
        Initialize_CSV();
        Initialize_Datas();
    }

    #endregion

    #region "Initialize"

    private void Initialize_CSV()
    {
        csv_datas = CSVReader.Read("CSV/In_App_Purchase_Product_CSV");
    }

    private void Initialize_Datas()
    {
        foreach (var datas in csv_datas.Values)
        {
            In_App_Purchase_Product_Data new_data = new In_App_Purchase_Product_Data();

            new_data.product_name = datas["product_name"].ToString();
            new_data.reward_types = datas["reward_type"].ToString().Split(";");
            new_data.product_type = (ProductType)System.Enum.Parse(typeof(ProductType), datas["product_type"].ToString());

            string[] reward_amounts_texts = datas["reward_amount"].ToString().Split(";");
            double[] reward_amounts = new double[reward_amounts_texts.Length];

            for (int i = 0; i < reward_amounts_texts.Length; i++)
            {
                reward_amounts[i] = double.Parse(reward_amounts_texts[i]);
            }

            new_data.reward_amounts = reward_amounts;

            product_datas.Add(new_data);
        }
    }

    #endregion

    #region "Get"

    public In_App_Purchase_Product_Data Get_Data(string product_name)
    {
        foreach (var product_data in product_datas)
        {
            if (product_data.product_name.Equals(product_name))
            {
                return product_data;
            }
        }

        return null;
    }

    #endregion
}