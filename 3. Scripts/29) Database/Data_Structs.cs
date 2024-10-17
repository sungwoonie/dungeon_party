using System.Collections.Generic;

[System.Serializable]
public struct Server_User_Data
{
    public string uuid;
    public bool ar;
    public string ot;
    public bool ab; //auto buff

    public void Initialize()
    {
        uuid = Back_End_Controller.instance.uuid;
        ar = false;
        ot = Back_End_Controller.instance.server_time.Change_To_String();
        ab = false;
    }
}

[System.Serializable]
public struct Equipped
{
    public string[] ec; //equiped_class
    public string[] es; //equiped_skill

    public string[] ee_0;
    public string[] ee_1;
    public string[] ee_2;

    public void Initialize()
    {
        ec = new string[3] { "Class_0_1_0", "Class_1_1_0", "Class_2_1_0" };
        es = new string[5];
        ee_0 = new string[4];
        ee_1 = new string[4];
        ee_2 = new string[4];
    }
}

[System.Serializable]
public struct Server_Stage_Data
{
    public int[] s; //stage
    public int[] hs; //high stage
    public int g; //gage

    public void Initialize()
    {
        s = new int[2] { 1, 1 };
        hs = new int[2] { 1, 1 };
        g = 0;
    }
}

[System.Serializable]
public struct Server_Battle_Pass_Data
{
    public int[] bpr; //battle pass requirement
    public int[] bpp; //battle pass purchased

    public int[] bprd_0; //battle pass received 0
    public int[] bprd_1; //battle pass received 0
    public int[] bprd_2; //battle pass received 0
    public int[] bprd_3; //battle pass received 0
    public int[] bprd_4; //battle pass received 0

    public void Initialize()
    {
        bpp = new int[5];
        bpr = new int[5];
        bprd_0 = new int[2];
        bprd_1 = new int[2];
        bprd_2 = new int[2];
        bprd_3 = new int[2];
        bprd_4 = new int[2];
    }
}

[System.Serializable]
public struct Server_Level_Data
{
    public int l; //level
    public string e; //exp

    public void Initialize()
    {
        l = 1;
        e = "0";
    }
}

[System.Serializable]
public struct Server_Quest_Data
{
    public string[] lqt; //latest_quest_time
    public int[] dr; //daily_requirement
    public int[] mr; //monthly_requirement

    public int[] drd; //daily received
    public int[] mrd; //monthly received

    public void Initialize()
    {
        lqt = new string[2];
        lqt[0] = Back_End_Controller.instance.server_time.Change_To_String();
        lqt[1] = Back_End_Controller.instance.server_time.Change_To_String();
        dr = new int[6];
        mr = new int[7];
        drd = new int[6];
        mrd = new int[7];
    }
}

[System.Serializable]
public struct Server_Daily_Data
{
    public string ld; //last_day
    public int c; //completed

    public int dr; //daily received
    public int wr; //weekly received

    public void Initialize()
    {
        ld = System.DateTime.MinValue.Change_To_String();
        dr = 0;
        wr = 0;
        c = 0;
    }
}

[System.Serializable]
public struct Scriptable_Datas
{
    public string growth_stats;
    public string ability_stats;
    public string beyond_stats;

    public string equipment_stats;
    public string skill_stats;
    public string class_stats;

    public void Initialize()
    {
        growth_stats = Scriptable_Data_Manager.instance.Get_Upgradable_Stat("growth_stats");
        ability_stats = Scriptable_Data_Manager.instance.Get_Upgradable_Stat("ability_stats");
        beyond_stats = Scriptable_Data_Manager.instance.Get_Upgradable_Stat("beyond_stats");

        equipment_stats = Scriptable_Data_Manager.instance.Get_Paid_Stat("equipment_stats");
        skill_stats = Scriptable_Data_Manager.instance.Get_Paid_Stat("skill_stats");
        class_stats = Scriptable_Data_Manager.instance.Get_Paid_Stat("class_stats");
    }

    public Scriptable_Datas(string _gs, string _as, string _bs, string _es, string _ss, string _cs)
    {
        growth_stats = _gs;
        ability_stats = _as;
        beyond_stats = _bs;

        equipment_stats = _es;
        skill_stats = _ss;
        class_stats = _cs;
    }
}

[System.Serializable]
public struct Server_Budget_Data
{
    public string gold;
    public string beyond_stone;
    public string enhance_stone;
    public string ability_stone;
    public string diamond;
    public string key;

    public List<double> paid_diamond;

    public void Initialize()
    {
        gold = "0";
        beyond_stone = "0";
        enhance_stone = "0";
        ability_stone = "0";
        diamond = "0";
        key = "0";
        paid_diamond = new List<double>();

    }

    public Server_Budget_Data(Server_Budget_Data data)
    {
        gold = data.gold.FromCurrencyString().ToCurrencyString();
        beyond_stone = data.beyond_stone.FromCurrencyString().ToCurrencyString();
        enhance_stone = data.enhance_stone.FromCurrencyString().ToCurrencyString();
        ability_stone = data.ability_stone.FromCurrencyString().ToCurrencyString();
        diamond = data.diamond.FromCurrencyString().ToCurrencyString();
        key = data.key.FromCurrencyString().ToCurrencyString();

        paid_diamond = data.paid_diamond;
    }

    public Server_Budget_Data(Budget data)
    {
        gold = data.gold.ToCurrencyString();
        beyond_stone = data.beyond_stone.ToCurrencyString();
        enhance_stone = data.enhance_stone.ToCurrencyString();
        ability_stone = data.ability_stone.ToCurrencyString();
        diamond = data.diamond.ToCurrencyString();
        key = data.key.ToCurrencyString();

        paid_diamond = data.paid_diamond;
    }
}