using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chatting_List : SingleTon<Chatting_List>
{
    private Chatting_Message[] chatting_messages;
    private List<Chatting_Message> current_showing_chatting_messages;

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
        chatting_messages = GetComponentsInChildren<Chatting_Message>(true);
        current_showing_chatting_messages = new List<Chatting_Message>();
    }

    #endregion

    #region "Set Chatting Message"

    public void Show_New_Message(string message, string user_name)
    {
        Chatting_Message new_chatting_message = Get_Chatting_Message();

        if (new_chatting_message != null)
        {
            new_chatting_message.Set_Chatting_Message(message, user_name);
        }
    }

    public void Remove_Message(string message, string user_name)
    {
        Chatting_Message remove_target = null;

        foreach (var current_showing_chatting_message in current_showing_chatting_messages)
        {
            if (current_showing_chatting_message.user_name_text.text.Equals(user_name))
            {
                if (current_showing_chatting_message.message_text.text.Equals(message))
                {
                    remove_target = current_showing_chatting_message;
                    break;
                }
            }
        }

        if (remove_target != null)
        {
            remove_target.gameObject.SetActive(false);
            current_showing_chatting_messages.Remove(remove_target);
        }
    }

    private Chatting_Message Get_Chatting_Message()
    {
        foreach (var chatting_message in chatting_messages)
        {
            if (!chatting_message.gameObject.activeSelf)
            {
                current_showing_chatting_messages.Add(chatting_message);
                return chatting_message;
            }
        }

        if (current_showing_chatting_messages.Count > 0)
        {
            Chatting_Message new_chatting_message = current_showing_chatting_messages[0];
            current_showing_chatting_messages.Remove(new_chatting_message);

            return new_chatting_message;
        }
        else
        {
            return null;
        }
    }

    #endregion
}
