using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System.Threading.Tasks;

public class Ranking_Manager : SingleTon<Ranking_Manager>
{
    private readonly string cpp_ranking_uuid = "57567810-355d-11ef-a568-37ae40c56abb";
    private readonly string lv_ranking_uuid = "db96e060-3562-11ef-add8-d52719a42b41";

    private string ranking_indate;

    private Ranking my_cpp_ranking;

    private bool test;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        test = Back_End_Controller.instance.test;
    }

    #endregion

    #region "Get Ranking"

    private Ranking Get_My_Ranking(bool cpp)
    {
        if (test)
        {
            return null;
        }

        Ranking my_rankning = new Ranking();

        BackendReturnObject return_ranking = Backend.URank.User.GetMyRank(cpp ? cpp_ranking_uuid : lv_ranking_uuid);

        if (return_ranking.IsSuccess())
        {
            LitJson.JsonData rank_data = return_ranking.GetFlattenJSON();

            my_rankning.rank = int.Parse(rank_data["rows"][0]["rank"].ToString());
            my_rankning.ranking_amount = rank_data["rows"][0]["score"].ToString();
            my_rankning.user_name = rank_data["rows"][0]["nickname"].ToString();

            Debug_Manager.Debug_Server_Message($"Got my {cpp} rank. rank is {my_rankning.rank}, score is {my_rankning.ranking_amount}");
            
            if (cpp)
            {
                my_cpp_ranking = my_rankning;
            }

            return my_rankning;
        }
        else
        {
            Debug.Log($"{return_ranking.GetErrorCode()}, {return_ranking.GetMessage()}, get my ranking {cpp}");

            return null;
        }
    }

    public List<Ranking> Get_Ranking_List(bool cpp)
    {
        if (test)
        {
            return null;
        }

        List<Ranking> ranking_list = new List<Ranking>();
        int limit = 6;
        BackendReturnObject return_ranking = Backend.URank.User.GetRankList(cpp ? cpp_ranking_uuid : lv_ranking_uuid, limit);

        if (return_ranking.IsSuccess())
        {
            LitJson.JsonData rank_data = return_ranking.GetFlattenJSON();

            for (int i = 0; i < rank_data["rows"].Count; i++)
            {
                Ranking new_ranking = new Ranking();

                new_ranking.rank = int.Parse(rank_data["rows"][i]["rank"].ToString());
                new_ranking.ranking_amount = rank_data["rows"][i]["score"].ToString();
                new_ranking.user_name = rank_data["rows"][i]["nickname"].ToString();

                ranking_list.Add(new_ranking);
            }

            Debug_Manager.Debug_Server_Message($"Get {cpp} Ranking list is success. Count is {rank_data.Count}");
        }
        else
        {
            Debug.Log($"{return_ranking.GetErrorCode()}, {return_ranking.GetMessage()}, get my ranking list {cpp}");
            return null;
        }

        return ranking_list;
    }

    #endregion

    #region "Update Ranking"

    public async void Start_Update_Ranking()
    {
        if (test)
        {
            return;
        }

        if (Internet_Pop_Up.instance.Pop_Up_Activating())
        {
            StartCoroutine(Internet_Pop_Up.instance.Wait_For_Internet(Start_Update_Ranking));
            return;
        }

        if (Party_Level_Manager.instance.Get_Party_Level() < 15)
        {
            Debug_Manager.Debug_Server_Message($"Can't Update ranking. cause Party Level is under 15");
            return;
        }

        await Update_Ranking();
    }

    private async Task Update_Ranking()
    {
        Ranking_Board.instance.Set_Lock_Object(false);

        Task update_cpp_ranking_task = Task.Run(() => Update_Ranking_Data(true));
        Task update_lv_ranking_task = Task.Run(() => Update_Ranking_Data(false));

        await Task.WhenAll(update_cpp_ranking_task, update_lv_ranking_task);

        Set_Ranking_Board();
    }

    private void Update_Ranking_Data(bool cpp)
    {
        if (test)
        {
            return;
        }

        if (string.IsNullOrEmpty(ranking_indate))
        {
            Debug_Manager.Debug_Server_Message("Rank indate is null");
            ranking_indate = Get_Ranking_Indate();
        }

        Param param = new Param();
        param.Add(cpp ? "CPP" : "LV", cpp ? Stat_Manager.instance.Combat_Power() : Party_Level_Manager.instance.Get_Party_Level());

        var bro = Backend.URank.User.UpdateUserScore(cpp ? cpp_ranking_uuid : lv_ranking_uuid, "Ranking", ranking_indate, param);

        if (bro.IsSuccess())
        {
            Debug_Manager.Debug_Server_Message($"Update My {cpp} Ranking score to {param.Values} is Success.");
        }
        else
        {
            Error_Handler(bro.GetErrorCode(), bro.GetMessage(), $"Update {cpp} ranking");
        }
    }

    #endregion

    #region "Indate"

    private string Get_Ranking_Indate()
    {
        var backend_return_object = Backend.GameData.Get("Ranking", new Where());

        if (backend_return_object.IsSuccess())
        {
            if (backend_return_object.FlattenRows().Count > 0)
            {
                Debug_Manager.Debug_Server_Message($"Get Ranking indate is success. indate is {backend_return_object.FlattenRows()[0]["inDate"]}");
                return backend_return_object.FlattenRows()[0]["inDate"].ToString();
            }
        }

        Debug_Manager.Debug_Server_Message($"Get Ranking indate is failed.");
        Error_Handler(backend_return_object.GetErrorCode(), backend_return_object.GetMessage(), $"Get Ranking Indate");
        return null;
    }

    #endregion

    #region "Set Ranking Board"

    public async void Set_Ranking_Board()
    {
        if (test)
        {
            return;
        }

        Ranking my_cpp_ranking = null;
        Task my_cpp_ranking_task = Task.Run(() => my_cpp_ranking = Get_My_Ranking(true));

        Ranking my_lv_ranking = null;
        Task my_lv_ranking_task = Task.Run(() => my_lv_ranking = Get_My_Ranking(false));

        List<Ranking> new_cpp_ranking_list = new List<Ranking>();
        Task new_cpp_ranking_list_task = Task.Run(() => new_cpp_ranking_list = Get_Ranking_List(true));

        List<Ranking> new_lv_ranking_list = new List<Ranking>();
        Task new_lv_ranking_list_task = Task.Run(() => new_lv_ranking_list = Get_Ranking_List(false));

        await Task.WhenAll(my_cpp_ranking_task, my_lv_ranking_task, new_cpp_ranking_list_task, new_lv_ranking_list_task);

        if (my_cpp_ranking == null)
        {
            new_cpp_ranking_list_task = Task.Run(() => Update_Ranking_Data(true));
            await Task.WhenAll(new_cpp_ranking_list_task);
            my_cpp_ranking_task = Task.Run(() => my_cpp_ranking = Get_My_Ranking(true));
            await Task.WhenAll(my_cpp_ranking_task);
        }

        if (my_lv_ranking == null)
        {
            new_lv_ranking_list_task = Task.Run(() => Update_Ranking_Data(false));
            await Task.WhenAll(new_lv_ranking_list_task);
            my_lv_ranking_task = Task.Run(() => my_lv_ranking = Get_My_Ranking(false));
            await Task.WhenAll(my_lv_ranking_task);
        }

        Ranking_Board.instance.Set_Ranking_Board(true, new_cpp_ranking_list, my_cpp_ranking);
        Ranking_Board.instance.Set_Ranking_Board(false, new_lv_ranking_list, my_lv_ranking);
    }

    #endregion

    #region "Error Handler"

    private void Error_Handler(string error_code, string error_message, string type)
    {
        Debug_Manager.Debug_Server_Message($"Got Error on Ranking. code is {error_code}, message is {error_message}, type is {type}");
    }

    #endregion

    #region "Get"

    public string Get_My_Ranking()
    {
        if (my_cpp_ranking != null)
        {
            return my_cpp_ranking.rank.ToString();
        }
        else
        {
            return "Newbie";
        }
    }

    #endregion
}
