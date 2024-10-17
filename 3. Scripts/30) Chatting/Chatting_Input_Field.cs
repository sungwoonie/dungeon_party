using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chatting_Input_Field : MonoBehaviour
{
    private TMP_InputField chatting_input_field;
    private Button send_button;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        chatting_input_field = GetComponent<TMP_InputField>();

        send_button = GetComponentInChildren<Button>(true);
        send_button.onClick.AddListener(() => Send_Message());
    }

    #endregion

    #region "Send Button"

    private void Send_Message()
    {
        string inputted_text = chatting_input_field.text;

        if (string.IsNullOrEmpty(inputted_text))
        {
            Debug_Manager.Debug_Server_Message($"Can't send message. inputted text is empty");
            Error_Message.instance.Set_Error_Message("Error_Message_Chatting_Empty");
            return;
        }

        if (Bad_Word_Censor.Contains_Bad_Word(inputted_text))
        {
            Debug_Manager.Debug_Server_Message($"Can't send message. inputted text contains bad word");
            Error_Message.instance.Set_Error_Message("Error_Message_Chatting_Contain_Badword");
            return;
        }
        else
        {
            if (inputted_text.Contains("_"))
            {
                Debug_Manager.Debug_Server_Message($"Can't send message. inputted text contains _");
                Error_Message.instance.Set_Error_Message("Error_Message_Chatting_Contain_Underbar");
                return;
            }

            Debug_Manager.Debug_Server_Message($"{inputted_text} Message sent");
            Chatting_Manager.instance.Send_Message(inputted_text);
            chatting_input_field.text = string.Empty;
        }
    }

    #endregion
}
