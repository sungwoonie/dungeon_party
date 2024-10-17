using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;

public class In_App_Purchase_Manager : SingleTon<In_App_Purchase_Manager>, IDetailedStoreListener
{
    private In_App_Purchase_Product_Data_Manager data_manager;

    private In_App_Purchase_Product_Data current_data;

    private IStoreController store_controller;
    private IExtensionProvider store_extension_provider;

    private bool can_purchase;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initialize IAP"

    private bool Is_Initialized()
    {
        return store_controller != null && store_extension_provider != null;
    }

    public void Initialize_In_App_Purchasing()
    {
        if (Is_Initialized()) return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

#if UNITY_ANDROID

        foreach (var product in data_manager.product_datas)
        {
            builder.AddProduct(product.product_name, product.product_type, new IDs
            {
                { product.product_name, GooglePlay.Name },
            });
        }

#endif

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        store_controller = controller;

#if UNITY_ANDROID

        foreach (var product in data_manager.product_datas)
        {
            var product_value = controller.products.WithID(product.product_name);

            if (product_value != null && product_value.availableToPurchase)
            {
            }
            else
            {
            }
        }

#endif

        Debug_Manager.Debug_Server_Message("In App Purchase Initialize success");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug_Manager.Debug_Server_Message("In App Purchase Initialize failed " + error.ToString());
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        data_manager = GetComponent<In_App_Purchase_Product_Data_Manager>();
    }

    #endregion

    #region "Purchase"

    public void Start_Purchase_Product(string product_name)
    {
        can_purchase = true;

        current_data = data_manager.Get_Data(product_name);

        if (store_controller == null)
        {
            Debug.Log("Purchase failed : In App Purchase Initialize failed");
        }

#if UNITY_ANDROID

        store_controller.InitiatePurchase(current_data.product_name);

#endif
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        if (!can_purchase)
        {
            return PurchaseProcessingResult.Complete;
        }

        bool valid_purchase = true;

#if UNITY_EDITOR

#else

        var validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
        try
        {
            var result = validator.Validate(purchaseEvent.purchasedProduct.receipt);

            foreach (IPurchaseReceipt purchase_receipt in result)
            {
                Debug.Log(purchase_receipt.productID);
                Debug.Log(purchase_receipt.purchaseDate);
                Debug.Log(purchase_receipt.transactionID);
            }
        }
        catch (IAPSecurityException)
        {
            Debug.Log("Invalid receipt");

            valid_purchase = false;
        }

#endif

        if (valid_purchase)
        {
            Give_Reward();
        }
        else
        {
            Debug_Manager.Debug_Server_Message("Purchase failed : invalid purchase");
        }

        can_purchase = false;

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        if (!product.Equals(PurchaseFailureReason.UserCancelled))
        {
            Debug.Log("purchase failed " + failureReason);
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
    }

#endregion

    #region "Give Reward"

    private void Give_Reward()
    {
        string[] reward_types = current_data.reward_types;
        double[] reward_amounts = current_data.reward_amounts;

        for (int i = 0; i < reward_types.Length; i++)
        {
            if (Budget_Manager.instance.Is_Budget(reward_types[i]))
            {
                if (reward_types[i].Equals("diamond"))
                {
                    Budget_Manager.instance.Get_Paid_Diamond(reward_amounts[i]);
                }
                else
                {
                    Budget_Manager.instance.Earn_Budget(reward_types[i], reward_amounts[i]);
                }

                continue;
            }

            if (Reward_Is_Battle_Pass(reward_types[i], out string pass_name))
            {
                Battle_Pass_Manager.instance.Battle_Pass_Purchased(true, pass_name);
                continue;
            }

            switch (reward_types[i])
            {
                case "ad_remove":
                    User_Data.instance.Save_Data_Ad_Remove(true);
                    break;
                case "auto_buff":
                    User_Data.instance.Save_Data_Auto_Buff(true);
                    Skill_Buff_Manager.instance.Use_All_AD_Buff();
                    break;
            }
        }

        Database_Controller.instance.Update_All_Data_To_Server_Asynch();
    }

    #endregion

    #region "Battle Pass"

    private bool Reward_Is_Battle_Pass(string reward_type, out string pass_name)
    {
        string[] splited_reward_type = reward_type.Split("_");

        foreach (var splited_type in splited_reward_type)
        {
            if (splited_type.Equals("battle"))
            {
                pass_name = Get_Pass_Name(splited_reward_type);
                return true;
            }
        }

        pass_name = string.Empty;
        return false;
    }

    private string Get_Pass_Name(string[] splited_reward_type)
    {
        string pass_name = string.Empty;

        for (int i = 0; i < splited_reward_type.Length; i++)
        {
            if (splited_reward_type[i].Equals("battle") || splited_reward_type[i].Equals("pass"))
            {
                break;
            }

            if (i >= 1)
            {
                pass_name += "_";
            }

            pass_name += splited_reward_type[i];
        }

        return pass_name;
    }

    #endregion

    #region "Price Text"

    private static System.Globalization.CultureInfo GetCultureInfoFromISOCurrencyCode(string code)
    {
        foreach (System.Globalization.CultureInfo ci in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures))
        {
            System.Globalization.RegionInfo ri = new System.Globalization.RegionInfo(ci.LCID);
            if (ri.ISOCurrencySymbol == code)
                return ci;
        }
        return null;
    }


    public string Get_Product_Local_Price(string product_ID)
    {
        Product product = store_controller.products.WithID(product_ID);

        System.Globalization.CultureInfo culture = GetCultureInfoFromISOCurrencyCode(product.metadata.isoCurrencyCode);

        if (culture != null)
        {
            return product.metadata.localizedPrice.ToString("C", culture);
        }
        else
        {
            return product.metadata.localizedPrice.ToString();
        }
    }

    #endregion
}