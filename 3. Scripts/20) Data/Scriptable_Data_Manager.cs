using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Scriptable_Data_Manager : SingleTon<Scriptable_Data_Manager>
{
    public List<ScriptableObject> growth_stats;
    public List<ScriptableObject> ability_stats;
    public List<ScriptableObject> beyond_stats;

    public List<ScriptableObject> equipment_stats;
    public List<ScriptableObject> skill_stats;
    public List<ScriptableObject> class_stats;

    protected override void Awake()
    {
        base.Awake();

        Initialize();
    }

    #region "Initialize"

    private void Initialize()
    {
        List<List<ScriptableObject>> upgradable_stats = new List<List<ScriptableObject>>()
        {
            growth_stats,
            ability_stats,
            beyond_stats
        };

        List<List<ScriptableObject>> paid_stats = new List<List<ScriptableObject>>()
        {
            equipment_stats,
            skill_stats,
            class_stats
        };

        foreach (var upgradable_stat in upgradable_stats)
        {
            foreach (var stat in upgradable_stat)
            {
                Upgradable_Stat new_stat = stat as Upgradable_Stat;

                if (new_stat != null)
                {
                    new_stat.level = 0;
                }
            }
        }

        foreach (var paid_stat in paid_stats)
        {
            foreach (var stat in paid_stat)
            {
                Paid_Stat new_stat = stat as Paid_Stat;

                if (new_stat != null)
                {
                    new_stat.level = 1;
                    new_stat.having_count = 0;
                }
            }
        }
    }

    #endregion

    #region "Server"

    public void Load_Scriptable_Datas_With_Server(Scriptable_Datas datas)
    {
        Load_Upgradable_Stats_With_Server(datas.growth_stats, datas.ability_stats, datas.beyond_stats);
        Load_Paid_Stats_With_Server(datas.equipment_stats, datas.skill_stats, datas.class_stats);
    }

    private void Load_Upgradable_Stats_With_Server(string growth_stat, string ability_stat, string beyond_stat)
    {
        Scriptable_Data[] growth_stat_list = Json_Helper.FromJson<Scriptable_Data>(growth_stat);
        Scriptable_Data[] gability_stat_list = Json_Helper.FromJson<Scriptable_Data>(ability_stat);
        Scriptable_Data[] beyond_stat_list = Json_Helper.FromJson<Scriptable_Data>(beyond_stat);

        foreach (Scriptable_Data data in growth_stat_list)
        {
            foreach (ScriptableObject growth_stat_data in growth_stats)
            {
                if (growth_stat_data.name == data.dn)
                {
                    Upgradable_Stat upgradable_stat = growth_stat_data as Upgradable_Stat;

                    if (upgradable_stat != null)
                    {
                        upgradable_stat.level = data.lv;
                    }
                }
            }
        }

        foreach (Scriptable_Data data in gability_stat_list)
        {
            foreach (ScriptableObject ability_stat_data in ability_stats)
            {
                if (ability_stat_data.name == data.dn)
                {
                    Upgradable_Stat upgradable_stat = ability_stat_data as Upgradable_Stat;

                    if (upgradable_stat != null)
                    {
                        upgradable_stat.level = data.lv;
                    }
                }
            }
        }

        foreach (Scriptable_Data data in beyond_stat_list)
        {
            foreach (ScriptableObject beyond_stat_data in beyond_stats)
            {
                if (beyond_stat_data.name == data.dn)
                {
                    Upgradable_Stat upgradable_stat = beyond_stat_data as Upgradable_Stat;

                    if (upgradable_stat != null)
                    {
                        upgradable_stat.level = data.lv;
                    }
                }
            }
        }
    }

    private void Load_Paid_Stats_With_Server(string equipment_stat, string skill_stat, string class_stat)
    {
        Scriptable_Data[] equipment_stat_list = Json_Helper.FromJson<Scriptable_Data>(equipment_stat);
        Scriptable_Data[] skill_stat_list = Json_Helper.FromJson<Scriptable_Data>(skill_stat);
        Scriptable_Data[] class_stat_list = Json_Helper.FromJson<Scriptable_Data>(class_stat);

        foreach (Scriptable_Data data in equipment_stat_list)
        {
            foreach (ScriptableObject equipment_stat_data in equipment_stats)
            {
                if (equipment_stat_data.name == data.dn)
                {
                    Paid_Stat paid_stat = equipment_stat_data as Paid_Stat;

                    if (paid_stat != null)
                    {
                        paid_stat.level = data.lv;
                        paid_stat.having_count = data.hc;
                    }
                }
            }
        }

        foreach (Scriptable_Data data in skill_stat_list)
        {
            foreach (ScriptableObject skill_stat_data in skill_stats)
            {
                if (skill_stat_data.name == data.dn)
                {
                    Paid_Stat paid_stat = skill_stat_data as Paid_Stat;

                    if (paid_stat != null)
                    {
                        paid_stat.level = data.lv;
                        paid_stat.having_count = data.hc;
                    }
                }
            }
        }

        foreach (Scriptable_Data data in class_stat_list)
        {
            foreach (ScriptableObject class_stat_data in class_stats)
            {
                if (class_stat_data.name == data.dn)
                {
                    Paid_Stat paid_stat = class_stat_data as Paid_Stat;

                    if (paid_stat != null)
                    {
                        paid_stat.level = data.lv;
                        paid_stat.having_count = data.hc;
                    }
                }
            }
        }
    }

    #endregion

    #region "Client"

    public Scriptable_Datas Load_All_Stat_Data_With_Client()
    {
        List<List<ScriptableObject>> all_stats = new List<List<ScriptableObject>>();
        all_stats.Add(growth_stats);
        all_stats.Add(ability_stats);
        all_stats.Add(beyond_stats);
        all_stats.Add(equipment_stats);
        all_stats.Add(skill_stats);
        all_stats.Add(class_stats);

        foreach (var all_stat in all_stats)
        {
            foreach (ScriptableObject scriptable_object in all_stat)
            {
                string json_data = Anti_Cheat_Manager.instance.Get(scriptable_object.name, "");

                if (!string.IsNullOrEmpty(json_data))
                {
                    Scriptable_Data scriptable_data = JsonUtility.FromJson<Scriptable_Data>(json_data);

                    Paid_Stat paid_stat = scriptable_object as Paid_Stat;

                    if (paid_stat != null)
                    {
                        paid_stat.level = scriptable_data.lv;
                        paid_stat.having_count = scriptable_data.hc;
                    }
                    else
                    {
                        Upgradable_Stat upgradable_stat = scriptable_object as Upgradable_Stat;

                        if (upgradable_stat != null)
                        {
                            upgradable_stat.level = scriptable_data.lv;
                        }
                    }
                }
            }
        }

        string _gs = Get_Upgradable_Stat("growth_stats");
        string _as = Get_Upgradable_Stat("ability_stats");
        string _bs = Get_Upgradable_Stat("beyond_stats");

        string _es = Get_Paid_Stat("equipment_stats");
        string _ss = Get_Paid_Stat("skill_stats");
        string _cs = Get_Paid_Stat("class_stats");

        return new Scriptable_Datas(_gs, _as, _bs, _es, _ss, _cs);
    }

    public void Save_Scriptable_Data_To_Client(ScriptableObject target_data)
    {
        List<List<ScriptableObject>> all_stats = new List<List<ScriptableObject>>();
        all_stats.Add(growth_stats);
        all_stats.Add(ability_stats);
        all_stats.Add(beyond_stats);
        all_stats.Add(equipment_stats);
        all_stats.Add(skill_stats);
        all_stats.Add(class_stats);

        foreach (var all_stat in all_stats)
        {
            if (all_stat.Contains(target_data))
            {
                Scriptable_Data new_data = new Scriptable_Data();

                Paid_Stat paid_stat = target_data as Paid_Stat;

                if (paid_stat != null)
                {
                    new_data.lv = paid_stat.level;
                    new_data.hc = paid_stat.having_count;
                }
                else
                {
                    Upgradable_Stat upgradable_stat = target_data as Upgradable_Stat;

                    if (upgradable_stat != null)
                    {
                        new_data.lv = upgradable_stat.level;
                    }
                }

                new_data.dn = target_data.name;

                string save_data = JsonUtility.ToJson(new_data);

                Anti_Cheat_Manager.instance.Set(new_data.dn, save_data);
                break;
            }
        }
    }

    public void Save_All_Scriptable_Data_To_Client()
    {
        List<List<ScriptableObject>> all_stats = new List<List<ScriptableObject>>();
        all_stats.Add(growth_stats);
        all_stats.Add(ability_stats);
        all_stats.Add(beyond_stats);
        all_stats.Add(equipment_stats);
        all_stats.Add(skill_stats);
        all_stats.Add(class_stats);

        foreach (var all_stat in all_stats)
        {
            foreach (var stat in all_stat)
            {
                Scriptable_Data new_data = new Scriptable_Data();
                new_data.dn = stat.name;

                Paid_Stat paid_stat = stat as Paid_Stat;

                if (paid_stat != null)
                {
                    new_data.lv = paid_stat.level;
                    new_data.hc = paid_stat.having_count;
                }
                else
                {
                    Upgradable_Stat upgradable_stat = stat as Upgradable_Stat;

                    if (upgradable_stat != null)
                    {
                        new_data.lv = upgradable_stat.level;
                        new_data.hc = 0;
                    }
                }

                string save_data = JsonUtility.ToJson(new_data);

                Anti_Cheat_Manager.instance.Set(new_data.dn, save_data);
            }
        }
    }

    #endregion

    #region "Get"

    public string Get_Upgradable_Stat(string name)
    {
        List<Scriptable_Data> scriptableDataList = new List<Scriptable_Data>();

        var upgradable_stats = GetType().GetField(name).GetValue(this) as List<ScriptableObject>;

        foreach (ScriptableObject upgradable_stat in upgradable_stats)
        {
            Upgradable_Stat upgradable_stat_data = upgradable_stat as Upgradable_Stat;

            if (upgradable_stat_data != null)
            {
                Scriptable_Data data = new Scriptable_Data
                {
                    dn = upgradable_stat_data.name,
                    lv = upgradable_stat_data.level,
                    hc = 0 // ±âº» °ª
                };

                scriptableDataList.Add(data);
            }
        }

        string jsonData = Json_Helper.ToJson(scriptableDataList.ToArray());

        return jsonData;
    }

    public string Get_Paid_Stat(string name)
    {
        List<Scriptable_Data> scriptableDataList = new List<Scriptable_Data>();

        var upgradable_stats = GetType().GetField(name).GetValue(this) as List<ScriptableObject>;

        foreach (ScriptableObject paid_stat in upgradable_stats)
        {
            Paid_Stat paid_stat_data = paid_stat as Paid_Stat;

            if (paid_stat_data != null)
            {
                Scriptable_Data data = new Scriptable_Data
                {
                    dn = paid_stat_data.name,
                    lv = paid_stat_data.level,
                    hc = paid_stat_data.having_count      
                };

                scriptableDataList.Add(data);
            }
        }

        string jsonData = Json_Helper.ToJson(scriptableDataList.ToArray());

        return jsonData;
    }

    #endregion
}