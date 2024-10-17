using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BackEnd;

public class Database_Controller : SingleTon<Database_Controller>
{
    public User_Database user_database;

    private bool test;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Update_All_Data_To_Server_Asynch();
        }
    }

    #region "Initialize"

    public void Initialize_Database()
    {
        user_database = new User_Database();
        user_database.Initialize_Data();

        test = Back_End_Controller.instance.test;

        if (test)
        {
            user_database.Set_By_Client();
        }
        else
        {
            if (user_database.Client_Data_Exist())
            {
                user_database.Set_By_Client();
                Debug_Manager.Debug_Server_Message("Load Client Data");
            }
            else
            {
                Load_Server_Data();
                Debug_Manager.Debug_Server_Message("Load Server Data");
            }
        }

        user_database.Save_All_Data_To_Client();
    }

    #endregion

    #region "Load Server Data"

    public void Load_Server_Data()
    {
        Debug_Manager.Debug_Server_Message("Load Server Data Start");

        var bro = Backend.GameData.GetMyData("User_Data", new Where());
        if (!bro.IsSuccess())
        {
            Debug.Log(bro);
            Application.Quit();
            return;
        }

        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
        {
            Debug_Manager.Debug_Server_Message("First Login. Save First Data to Server Start");
            Write_New_Data();
            Debug_Manager.Debug_Server_Message("Save First Data to Server Finished");
        }
        else
        {
            Debug_Manager.Debug_Server_Message("Not First Login, Load Data from Server Start");

            Load_ETC_Data(out User_Database server_data);
            server_data.sd = Load_Scriptable_Data();

            user_database.Set_By_Server(server_data);

            Debug_Manager.Debug_Server_Message("Load Data from Server Finished");
        }

        Debug_Manager.Debug_Server_Message("Load Server Data Finished");
    }

    private void Load_ETC_Data(out User_Database server_data)
    {
        Debug_Manager.Debug_Server_Message("Load ETC Data from Server Start");

        server_data = null;

        var transaction_list = new List<TransactionValue>
        {
            TransactionValue.SetGet("Battle_Pass", new Where()),
            TransactionValue.SetGet("Budget", new Where()),
            TransactionValue.SetGet("Daily_Reward", new Where()),
            TransactionValue.SetGet("Level", new Where()),
            TransactionValue.SetGet("Quest", new Where()),
            TransactionValue.SetGet("Stage", new Where()),
            TransactionValue.SetGet("Beyond_Rank", new Where()),
            TransactionValue.SetGet("Equipped", new Where()),
            TransactionValue.SetGet("User_Data", new Where())
        };

        var etc_datas = Backend.GameData.TransactionReadV2(transaction_list);

        if (!etc_datas.IsSuccess())
        {
            Debug.LogError(etc_datas.ToString());
            return;
        }

        if (etc_datas.IsSuccess())
        {
            server_data = new User_Database();

            var data = etc_datas.GetFlattenJSON()["Responses"];
            server_data.bpd = JsonUtility.FromJson<Server_Battle_Pass_Data>(data[0]["BP"].ToJson());
            server_data.bd = new Server_Budget_Data(JsonUtility.FromJson<Server_Budget_Data>(data[1]["BD"].ToJson()));
            server_data.dd = JsonUtility.FromJson<Server_Daily_Data>(data[2]["DR"].ToJson());
            server_data.ld = JsonUtility.FromJson<Server_Level_Data>(data[3]["LV"].ToJson());
            server_data.qd = JsonUtility.FromJson<Server_Quest_Data>(data[4]["QS"].ToJson());
            server_data.s = JsonUtility.FromJson<Server_Stage_Data>(data[5]["STG"].ToJson());
            server_data.br = int.Parse(data[6]["BR"].ToString());
            server_data.e = JsonUtility.FromJson<Equipped>(data[7]["EQD"].ToJson());
            server_data.ud.ot = data[8]["OT"].ToString();
            server_data.ud.uuid = data[8]["UUID"].ToString();
            server_data.ud.ar = bool.Parse(data[8]["AR"].ToString());
            server_data.ud.ab = bool.Parse(data[8]["AB"].ToString());
        }

        Debug_Manager.Debug_Server_Message("Load ETC Data from Server Finished");
    }

    private Scriptable_Datas Load_Scriptable_Data()
    {
        Debug_Manager.Debug_Server_Message("Load Scriptable Data from Server Start");

        Scriptable_Datas server_data = new Scriptable_Datas();

        var transaction_list = new List<TransactionValue>
        {
            TransactionValue.SetGet("Growth_Stat", new Where()),
            TransactionValue.SetGet("Ability_Stat", new Where()),
            TransactionValue.SetGet("Beyond_Stat", new Where()),
            TransactionValue.SetGet("Equipment_Stat", new Where()),
            TransactionValue.SetGet("Skill_Stat", new Where()),
            TransactionValue.SetGet("Class_Stat", new Where())
        };

        var bro = Backend.GameData.TransactionReadV2(transaction_list);
        if (!bro.IsSuccess())
        {
            Debug.LogError(bro.ToString());
            return new Scriptable_Datas();
        }

        if (bro.IsSuccess())
        {
            var data = bro.GetFlattenJSON()["Responses"];

            server_data = new Scriptable_Datas(
                data[0]["GS"].ToString(),
                data[1]["AS"].ToString(),
                data[2]["BS"].ToString(),
                data[3]["ES"].ToString(),
                data[4]["SS"].ToString(),
                data[5]["CS"].ToString()
            );
        }

        Debug_Manager.Debug_Server_Message("Load Scriptable Data from Server is Finished");

        return server_data;
    }

    #endregion

    #region "Write Data"

    public void Write_New_Data()
    {
        Debug_Manager.Debug_Server_Message("Write Data to Server Start");

        var paid_stat_return_object = Backend.GameData.TransactionWriteV2(Get_Scriptable_Transaction_List(true));

        if (paid_stat_return_object.IsSuccess())
        {
            Debug_Manager.Debug_Server_Message($"Save Paid Stat Success");
        }
        else
        {
            Debug.Log(paid_stat_return_object.GetErrorCode());
        }

        var etc_return_object = Backend.GameData.TransactionWriteV2(Get_ETC_Transaction_List(true));

        if (etc_return_object.IsSuccess())
        {
            Debug_Manager.Debug_Server_Message($"Save ETC Stat Success");
        }
        else
        {
            Debug.Log(etc_return_object.GetErrorCode());
        }

        Debug_Manager.Debug_Server_Message("Save First Data to Server Finished");
    }

    #endregion

    #region "Update Data To Server"

    public void Update_All_Data_To_Server()
    {
        if (test)
        {
            return;
        }

        if (Internet_Pop_Up.instance.Pop_Up_Activating())
        {
            StartCoroutine(Internet_Pop_Up.instance.Wait_For_Internet(Update_All_Data_To_Server));
            return;
        }

        if (Party_Level_Manager.instance.Get_Party_Level() < 10)
        {
            Debug_Manager.Debug_Server_Message($"Can't save Data to Server. Cause Party Level is under 10");
            return;
        }

        Debug_Manager.Debug_Server_Message("Update Data to Server Start");

        user_database.Set_By_Client();

        Backend.GameData.TransactionWriteV2(Get_Scriptable_Transaction_List(false), (callback) =>
        {
            if (callback.IsSuccess())
            {
            }
            else
            {
                Debug.Log(callback.GetErrorCode());
            }
            // 捞饶 贸府
        });

        Backend.GameData.TransactionWriteV2(Get_ETC_Transaction_List(false), (callback) =>
        {
            if (callback.IsSuccess())
            {
            }
            else
            {
                Debug.Log(callback.GetErrorCode());
            }
            // 捞饶 贸府
        });

        Debug_Manager.Debug_Server_Message("Update Data to Server Finished");
    }

    public void Update_All_Data_To_Server_Asynch()
    {
        if (test)
        {
            return;
        }

        if (Internet_Pop_Up.instance != null)
        {
            if (Internet_Pop_Up.instance.Pop_Up_Activating())
            {
                StartCoroutine(Internet_Pop_Up.instance.Wait_For_Internet(Update_All_Data_To_Server_Asynch));
                return;
            }
        }

        Debug_Manager.Debug_Server_Message("Update Data to Server Start with Async");

        user_database.Set_By_Client();

        var paid_stat_return_object = Backend.GameData.TransactionWriteV2(Get_Scriptable_Transaction_List(false));

        if (paid_stat_return_object.IsSuccess())
        {
            Debug_Manager.Debug_Server_Message($"Save Paid Stat Success");
        }
        else
        {
            Debug.Log(paid_stat_return_object.GetErrorCode());
        }

        var etc_return_object = Backend.GameData.TransactionWriteV2(Get_ETC_Transaction_List(false));

        if (etc_return_object.IsSuccess())
        {
            Debug_Manager.Debug_Server_Message($"Save ETC Stat Success");
        }
        else
        {
            Debug.Log(etc_return_object.GetErrorCode());
            Debug.Log(etc_return_object.GetMessage());

        }

        Debug_Manager.Debug_Server_Message("Update Data to Server Finished");
    }

    #endregion

    #region "Param"

    private Param New_Param(string[] names, object[] datas)
    {
        var new_param = new Param();

        for (int i = 0; i < names.Length; i++)
        {
            new_param.Add(names[i], datas[i]);
        }

        return new_param;
    }

    private Param New_Param(string name, object data)
    {
        var new_param = new Param();
        new_param.Add(name, data);

        return new_param;
    }

    #endregion

    #region "Transaction List"

    private List<TransactionValue> Get_Scriptable_Transaction_List(bool insert)
    {
        var transaction_list = new List<TransactionValue>();

        if (insert)
        {
            transaction_list = new List<TransactionValue>()
            {
                TransactionValue.SetInsert("Growth_Stat", New_Param("GS", user_database.sd.growth_stats)),
                TransactionValue.SetInsert("Ability_Stat", New_Param("AS", user_database.sd.ability_stats)),
                TransactionValue.SetInsert("Beyond_Stat", New_Param("BS", user_database.sd.beyond_stats)),
                TransactionValue.SetInsert("Equipment_Stat", New_Param("ES", user_database.sd.equipment_stats)),
                TransactionValue.SetInsert("Skill_Stat", New_Param("SS", user_database.sd.skill_stats)),
                TransactionValue.SetInsert("Class_Stat", New_Param("CS", user_database.sd.class_stats))
            };
        }
        else
        {
            transaction_list = new List<TransactionValue>()
            {
                TransactionValue.SetUpdate("Growth_Stat", new Where(), New_Param("GS", user_database.sd.growth_stats)),
                TransactionValue.SetUpdate("Ability_Stat", new Where(), New_Param("AS", user_database.sd.ability_stats)),
                TransactionValue.SetUpdate("Beyond_Stat", new Where(), New_Param("BS", user_database.sd.beyond_stats)),
                TransactionValue.SetUpdate("Equipment_Stat", new Where(), New_Param("ES", user_database.sd.equipment_stats)),
                TransactionValue.SetUpdate("Skill_Stat", new Where(), New_Param("SS", user_database.sd.skill_stats)),
                TransactionValue.SetUpdate("Class_Stat", new Where(), New_Param("CS", user_database.sd.class_stats))
            };
        }

        return transaction_list;
    }

    private List<TransactionValue> Get_ETC_Transaction_List(bool insert)
    {
        string[] ranking_names = new string[2] { "CPP", "LV" };
        object[] ranking_valuse = new object[2] { 0, 1 };

        string[] user_data_names = new string[4] { "UUID", "AR", "OT", "AB" };
        object[] user_data_valuse = new object[4] { user_database.ud.uuid, user_database.ud.ar, user_database.ud.ot, user_database.ud.ab };

        var transaction_list = new List<TransactionValue>();

        if (insert)
        {
            transaction_list = new List<TransactionValue>()
            {
                TransactionValue.SetInsert("Budget", New_Param("BD", user_database.bd)),
                TransactionValue.SetInsert("Battle_Pass", New_Param("BP", user_database.bpd)),
                TransactionValue.SetInsert("Stage", New_Param("STG", user_database.s)),
                TransactionValue.SetInsert("Beyond_Rank", New_Param("BR", user_database.br)),
                TransactionValue.SetInsert("Level", New_Param("LV", user_database.ld)),
                TransactionValue.SetInsert("Daily_Reward", New_Param("DR", user_database.dd)),
                TransactionValue.SetInsert("Quest", New_Param("QS", user_database.qd)),
                TransactionValue.SetInsert("Equipped", New_Param("EQD", user_database.e)),
                TransactionValue.SetInsert("Ranking", New_Param(ranking_names, ranking_valuse)),
                TransactionValue.SetInsert("User_Data", New_Param(user_data_names, user_data_valuse))
            };
        }
        else
        {
            transaction_list = new List<TransactionValue>()
            {
                TransactionValue.SetUpdate("Budget", new Where(), New_Param("BD", user_database.bd)),
                TransactionValue.SetUpdate("Battle_Pass", new Where(), New_Param("BP", user_database.bpd)),
                TransactionValue.SetUpdate("Stage", new Where(), New_Param("STG", user_database.s)),
                TransactionValue.SetUpdate("Beyond_Rank", new Where(), New_Param("BR", user_database.br)),
                TransactionValue.SetUpdate("Level", new Where(), New_Param("LV", user_database.ld)),
                TransactionValue.SetUpdate("Daily_Reward", new Where(), New_Param("DR", user_database.dd)),
                TransactionValue.SetUpdate("Quest", new Where(), New_Param("QS", user_database.qd)),
                TransactionValue.SetUpdate("Equipped", new Where(), New_Param("EQD", user_database.e)),
                TransactionValue.SetUpdate("Ranking", new Where(), New_Param(ranking_names, ranking_valuse)),
                TransactionValue.SetUpdate("User_Data", new Where(), New_Param(user_data_names, user_data_valuse))
            };
        }

        return transaction_list;
    }

    #endregion
}