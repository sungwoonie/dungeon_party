using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exchange_Product_Data_Manager : SingleTon<Exchange_Product_Data_Manager>
{
    private Dictionary<string, Dictionary<string, object>> csv_datas = new Dictionary<string, Dictionary<string, object>>();
    private List<Exchange_Product_Data> product_datas = new List<Exchange_Product_Data>();

    private Exchange_Content[] exchange_contents;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_CSV();
    }

    private void Start()
    {
        Initialize_Contents();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Contents()
    {
        exchange_contents = GetComponentsInChildren<Exchange_Content>(true);

        foreach (var exchange_content in exchange_contents)
        {
            exchange_content.Initialize_Content();
        }
    }

    private void Initialize_CSV()
    {
        csv_datas = CSVReader.Read("CSV/Exchange_Product_CSV");

        foreach (var csv_data in csv_datas)
        {
            Exchange_Product_Data new_product_data = new Exchange_Product_Data();

            new_product_data.product_name = csv_data.Value["product_name"].ToString();
            new_product_data.reward_type = csv_data.Value["reward_type"].ToString();
            new_product_data.reward_amount = double.Parse(csv_data.Value["reward_amount"].ToString());
            new_product_data.price = double.Parse(csv_data.Value["price"].ToString());

            product_datas.Add(new_product_data);
        }
    }

    #endregion

    #region "Get Data"

    public Exchange_Product_Data Get_Product_Data(string product_name)
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
