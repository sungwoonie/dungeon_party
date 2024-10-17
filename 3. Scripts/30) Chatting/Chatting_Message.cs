using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chatting_Message : MonoBehaviour
{
    public TMP_Text user_name_text;
    public TMP_Text message_text;

    private Content_Fitter_Refresh content_fitter;

    #region "Unity"

    private void OnEnable()
    {
        Set_Size_Fitter();
    }

    #endregion

    #region "Size Fitter"

    private void Set_Size_Fitter()
    {
        if (content_fitter == null)
        {
            Initialize_Component();
        }

        content_fitter.Refresh_Content_Fitters();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        content_fitter = GetComponent<Content_Fitter_Refresh>();
    }

    #endregion

    #region "Set"

    public void Set_Chatting_Message(string message, string user_name)
    {
        string[] splited_message = message.Split("_");

        if (splited_message[0].Equals("System"))
        {
            user_name_text.text = Localization_Manager.instance.Get_Localized_String("System");
            string system = Localization_Manager.instance.Get_Localized_String(splited_message[1]);
            string system_message = Localization_Manager.instance.Get_Localized_String(splited_message[2]);

            if (system.Contains("_"))
            {
                message_text.text = $"{user_name}{system.Replace("_", system_message)}";
            }
            else
            {
                message_text.text = $"{system_message} {system}";
            }
        }
        else
        {
            user_name_text.text = $"[{Set_Rank_Text(splited_message[0])}] {user_name}";
            message_text.text = splited_message[1];
        }

        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }

    private string Set_Rank_Text(string rank)
    {
        string[] splited_text = rank.Split("_");
        string colored_rank = string.Empty;
        string color_code = string.Empty;

        if (int.TryParse(splited_text[0], out int parsed_rank))
        {
            switch (parsed_rank)
            {
                case 1:
                    color_code = "#FFD700";
                    break;
                case 2:
                    color_code = "#C0C0C0";
                    break;
                case 3:
                    color_code = "#624637";
                    break;
                default:
                    color_code = "#00FFFF";
                    break;
            }

            colored_rank = $"<color={color_code}>{parsed_rank}</color>";
        }
        else
        {
            color_code = "#00FF00";
            colored_rank = $"<color={color_code}>{Localization_Manager.instance.Get_Localized_String(rank)}</color>";
        }

        return colored_rank;
    }

    #endregion
}
