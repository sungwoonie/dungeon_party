using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

[System.Serializable]
public class In_App_Purchase_Product_Data
{
    public string product_name;
    public string[] reward_types;
    public double[] reward_amounts;
    public ProductType product_type;
}