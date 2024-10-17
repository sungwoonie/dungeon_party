using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BackEnd;

public class Coupon_Controller : SingleTon<Coupon_Controller>
{
    public GameObject error_message;
    public Localization_Text error_message_text;

    public Button check_button;

    private TMP_InputField input_field;
    private GameObject pop_up;
    private Coupon_Reward_Controller reward;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        pop_up = transform.GetChild(0).gameObject;

        check_button.onClick.AddListener(() => Claim_Button());
        input_field = GetComponentInChildren<TMP_InputField>(true);

        reward = GetComponent<Coupon_Reward_Controller>();
    }

    #endregion

    #region "Set"

    public void Set_Coupon(string coupon_code)
    {
        input_field.text = coupon_code;
        pop_up.SetActive(true);
    }

    #endregion

    #region "Claim"

    private void Claim_Button()
    {
        check_button.enabled = false;

        if (input_field.text == string.Empty)
        {
            Set_Error_Message("Coupon_Code_Is_Empty");
        }
        else
        {
            if (reward.Coupon_Exist(input_field.text))
            {
                var return_object = Backend.Coupon.UseCoupon(input_field.text);

                if (return_object.IsSuccess())
                {
                    reward.Give_Coupon_Reward(input_field.text);
                    Database_Controller.instance.Update_All_Data_To_Server_Asynch();
                    Set_Error_Message("Coupon_Used");
                }
                else
                {
                    Set_Error_Message("Coupon_Error");
                }
            }
            else
            {
                Set_Error_Message("Coupon_Not_Exist");
            }
        }

        check_button.enabled = true;
    }

    #endregion

    #region "Error"

    private void Set_Error_Message(string error_code)
    {
        StopAllCoroutines();
        StartCoroutine(Error_Message_Count());

        error_message_text.Set_Localization_Key(error_code);
        error_message_text.Localize_Text();

        error_message.SetActive(true);
    }

    private IEnumerator Error_Message_Count()
    {
        float show_timer = 5.0f;
        float current_timer = show_timer;

        while (current_timer > 0.0f)
        {
            current_timer -= Time.deltaTime;
            yield return null;
        }

        error_message.SetActive(false);
    }

    #endregion
}
