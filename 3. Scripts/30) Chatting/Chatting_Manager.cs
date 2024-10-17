using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackndChat;
using System;

public class Chatting_Manager : SingleTon<Chatting_Manager>, IChatClientListener
{
    private ChatClient ChatClient = null;

    private string channel_group;
    private string channel_name;
    private UInt64 channel_number;

    private bool test;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        test = Back_End_Controller.instance.test;
    }

    private void Start()
    {
        Initialize_Chatting();
    }

    private void Update()
    {
        Update_Chat_Client();
    }

    private void OnApplicationQuit()
    {
        Leave_Chatting_Server();
    }

    #endregion

    #region "Initialize"

    public void Initialize_Chatting()
    {
        if (test)
        {
            return;
        }

        ChatClient = new ChatClient(this, new ChatClientArguments
        {
            Avatar = "default"
        });
    }

    #endregion

    #region "Server Call Back"

    public void OnChatMessage(MessageInfo messageInfo)
    {
        Debug_Manager.Debug_Server_Message($"Got New '{messageInfo.Message}' message that {messageInfo.GamerName} sent");
        Chatting_List.instance.Show_New_Message(messageInfo.Message, messageInfo.GamerName);
    }

    public void OnDeleteMessage(MessageInfo messageInfo)
    {
        Debug_Manager.Debug_Server_Message($"Server Deleted '{messageInfo.Message}' message that {messageInfo.GamerName} sent");
        Chatting_List.instance.Remove_Message(messageInfo.Message, messageInfo.GamerName);
    }

    public void OnError(ERROR_MESSAGE error, object param)
    {
        Debug_Manager.Debug_Server_Message($"Chatting Server Error with {error}");
    }

    public void OnHideMessage(MessageInfo messageInfo)
    {
        Debug_Manager.Debug_Server_Message($"Server Hided '{messageInfo.Message}' message that {messageInfo.GamerName} sent");
        Chatting_List.instance.Remove_Message(messageInfo.Message, messageInfo.GamerName);
    }

    public void OnJoinChannel(ChannelInfo channelInfo)
    {
        channel_group = channelInfo.ChannelGroup;
        channel_name = channelInfo.ChannelName;
        channel_number = channelInfo.ChannelNumber;

        Debug_Manager.Debug_Server_Message($"Joined {channel_group}'s {channel_name} {channel_number}");
    }

    public void OnJoinChannelPlayer(string channelGroup, string channelName, ulong channelNumber, string gamerName, string avatar)
    {
        Debug_Manager.Debug_Server_Message($"{gamerName} Joined on {channelGroup}'s {channelName} {channelNumber}");
    }

    public void OnLeaveChannel(ChannelInfo channelInfo)
    {
        channel_group = channelInfo.ChannelGroup;
        channel_name = channelInfo.ChannelName;
        channel_number = channelInfo.ChannelNumber;

        Debug_Manager.Debug_Server_Message($"Leaved {channel_group}'s {channel_name} {channel_number}");
    }

    public void OnLeaveChannelPlayer(string channelGroup, string channelName, ulong channelNumber, string gamerName, string avatar)
    {
        Debug_Manager.Debug_Server_Message($"{gamerName} Leaved on {channelGroup}'s {channelName} {channelNumber}");
    }

    public void OnSuccess(SUCCESS_MESSAGE success, object param)
    {
        Debug_Manager.Debug_Server_Message($"Chatting Server Success with {success}");
    }

    public void OnTranslateMessage(List<MessageInfo> messages)
    {
    }

    public void OnWhisperMessage(WhisperMessageInfo messageInfo)
    {
    }

    #endregion

    #region "Set Chatting"

    private void Update_Chat_Client()
    {
        if (test)
        {
            return;
        }

        ChatClient?.Update();
    }

    private void Leave_Chatting_Server()
    {
        if (test)
        {
            return;
        }

        ChatClient?.Dispose();
    }

    #endregion

    #region "Send Message"

    public void Send_Message(string message_text)
    {
        if (test)
        {
            return;
        }

        if (!message_text.Split("_")[0].Equals("System"))
        {
            message_text = $"{Ranking_Manager.instance.Get_My_Ranking()}_{message_text}";
        }

        ChatClient.SendChatMessage(channel_group, channel_name, channel_number, message_text);
    }

    public void Send_System_Message(string message_text)
    {
        if (test)
        {
            return;
        }

        ChatClient.SendChatMessage(channel_group, channel_name, channel_number, message_text);
    }

    #endregion
}
