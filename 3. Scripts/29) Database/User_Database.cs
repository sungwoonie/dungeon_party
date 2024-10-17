using UnityEngine;

[System.Serializable]
public class User_Database
{
    public Server_User_Data ud;

    public Equipped e; //have to add equipment
    public Server_Stage_Data s; //server stage data
    public Server_Budget_Data bd; //budget

    public Scriptable_Datas sd; //scriptable_datas

    public int br; //beyond_rank

    public Server_Level_Data ld; //level data
    public Server_Daily_Data dd; //daily_data

    public Server_Quest_Data qd; //quest_data

    public Server_Battle_Pass_Data bpd; //battle_pass_data

    #region "Initialize"

    public void Initialize_Data()
    {
        ud.Initialize();
        e.Initialize();
        s.Initialize();
        sd.Initialize();
        ld.Initialize();
        dd.Initialize();
        qd.Initialize();
        bpd.Initialize();
        bd.Initialize();
        br = 0;
    }

    #endregion

    #region "Client"

    public bool Client_Data_Exist()
    {
        return Anti_Cheat_Manager.instance.Get("Client_Data_Exist", false);
    }

    public void Set_By_Client()
    {
        s = JsonUtility.FromJson<Server_Stage_Data>(Anti_Cheat_Manager.instance.Get("Stage", JsonUtility.ToJson(s)));
        bd = new Server_Budget_Data(JsonUtility.FromJson<Budget>(Anti_Cheat_Manager.instance.Get("Budget", JsonUtility.ToJson(new Budget(bd)))));
        sd = Scriptable_Data_Manager.instance.Load_All_Stat_Data_With_Client();
        br = int.Parse(Anti_Cheat_Manager.instance.Get("Beyond_Rank", "0"));
        ld = JsonUtility.FromJson<Server_Level_Data>(Anti_Cheat_Manager.instance.Get("Level_Data", JsonUtility.ToJson(ld)));
        dd = JsonUtility.FromJson<Server_Daily_Data>(Anti_Cheat_Manager.instance.Get("Daily_Data", JsonUtility.ToJson(dd)));
        qd = JsonUtility.FromJson<Server_Quest_Data>(Anti_Cheat_Manager.instance.Get("Quest_Data", JsonUtility.ToJson(qd)));
        bpd = JsonUtility.FromJson<Server_Battle_Pass_Data>(Anti_Cheat_Manager.instance.Get("Battle_Pass_Data", JsonUtility.ToJson(bpd)));
        e = JsonUtility.FromJson<Equipped>(Anti_Cheat_Manager.instance.Get("Equipped", JsonUtility.ToJson(e)));
        ud = JsonUtility.FromJson<Server_User_Data>(Anti_Cheat_Manager.instance.Get("User_Data", JsonUtility.ToJson(ud)));
    }

    public void Save_All_Data_To_Client()
    {
        Anti_Cheat_Manager.instance.Set("Stage", JsonUtility.ToJson(s));
        Anti_Cheat_Manager.instance.Set("Budget", JsonUtility.ToJson(new Budget(bd)));
        Anti_Cheat_Manager.instance.Set("Beyond_Rank", br.ToString());
        Anti_Cheat_Manager.instance.Set("Level_Data", JsonUtility.ToJson(ld));
        Anti_Cheat_Manager.instance.Set("Daily_Data", JsonUtility.ToJson(dd));
        Anti_Cheat_Manager.instance.Set("Quest_Data", JsonUtility.ToJson(qd));
        Anti_Cheat_Manager.instance.Set("Battle_Pass_Data", JsonUtility.ToJson(bpd));
        Anti_Cheat_Manager.instance.Set("Equipped", JsonUtility.ToJson(e));
        Anti_Cheat_Manager.instance.Set("User_Data", JsonUtility.ToJson(ud));

        Scriptable_Data_Manager.instance.Save_All_Scriptable_Data_To_Client();

        Anti_Cheat_Manager.instance.Set("Client_Data_Exist", true);
    }

    #endregion

    #region "Server"

    public void Set_By_Server(User_Database server_data)
    {
        ud = server_data.ud;
        e = server_data.e;
        s = server_data.s;
        bd = server_data.bd;
        br = server_data.br;
        ld = server_data.ld;
        dd = server_data.dd;
        qd = server_data.qd;
        bpd = server_data.bpd;

        sd = server_data.sd;
        Scriptable_Data_Manager.instance.Load_Scriptable_Datas_With_Server(server_data.sd);
    }

    #endregion
}