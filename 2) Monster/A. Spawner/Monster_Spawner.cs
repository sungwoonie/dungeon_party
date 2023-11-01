using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_Spawner : SingleTon<Monster_Spawner>
{
    [Header("Vector"), Space()]
    public Vector3 spawn_position;
    public Vector3 stay_position;

    [Header("Boss Alert"), Space()]
    public GameObject boss_alert_pop_up;
    public RectTransform[] boss_alert_text;

    private Object_Pooling[] monster_pool;

    #region "Event Bus"

    private void OnEnable()
    {
        Event_Bus.Subscribe_Event(Event_Type.Ready, Spawn_Normal);
    }

    private void OnDisable()
    {
        Event_Bus.UnSubscribe_Event(Event_Type.Ready, Spawn_Normal);
    }

    #endregion

    private void Start()
    {
        monster_pool = GetComponentsInChildren<Object_Pooling>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Event_Bus.Publish(Event_Type.Ready);
        }
    }

    public GameObject Get_Current_Monster()
    {
        List<GameObject> activating_monster = monster_pool[0].Get_Activating_Pool();
        List<GameObject> boss = monster_pool[1].Get_Activating_Pool();

        if (activating_monster.Count > 0)
        {
            return activating_monster[0];
        }
        else if (boss.Count > 0)
        {
            return boss[0];
        }
        else
        {
            return null;
        }
    }

    public void Spawn_Normal()
    {
        StartCoroutine(Spawn_Monster(false));
    }

    public void Spawn_Boss()
    {
        if (Stage_Gage.instance.current_gage >= 100)
        {
            Stage_Gage.instance.Set_Stage_Gage(0);

            StartCoroutine(Spawn_Monster(true));

            List<GameObject> activating_monster = monster_pool[0].Get_Activating_Pool();
            activating_monster[0].SetActive(false);
        }
    }

    private IEnumerator Spawn_Monster(bool is_boss)
    {
        if (is_boss)
        {
            boss_alert_pop_up.SetActive(true);

            Event_Bus.Publish(Event_Type.Boss_Spawn);
            yield return StartCoroutine(Boss_Alert());

            boss_alert_pop_up.SetActive(false);
        }
        else
        {
            yield return null;
        }

        int random_monster_count = 0; //Random.Range(0, 3);
        string monster_stat_code = Get_Monster_Stat_Code(is_boss);
        string monster_code = Get_Monster_Code(is_boss) + random_monster_count;
        Monster_Stat target_stat = Monster_Stat_Controller.instance.Get_Monster_Stat(monster_stat_code);
        Object_Pooling pool = monster_pool[is_boss ? 1 : 0];

        Transform new_monster = pool.Pool(monster_code);
        new_monster.position = is_boss ? stay_position : spawn_position;
        new_monster.gameObject.SetActive(true);
        new_monster.GetComponent<Monster_Behaviour>().Initialize_Monster(target_stat);
    }

    private IEnumerator Boss_Alert()
    {
        foreach (RectTransform text in boss_alert_text)
        {
            Vector3 base_position = new Vector3(0, text.position.y, text.position.z);
            text.position = base_position;
        }

        float timer = 0.0f;

        while (timer < 2.0f)
        {
            timer += Time.deltaTime;

            boss_alert_text[0].Translate(Vector3.left * Time.deltaTime);
            boss_alert_text[1].Translate(Vector3.right * Time.deltaTime);

            yield return null;
        }
    }

    #region "Get Code"

    private string Get_Monster_Code(bool is_boss)
    {
        int boss_code = is_boss ? 1 : 0;
        int current_stage = (Stage_Manager.instance.Get_Stage / 50) + 1;

        string monster_code = "Monster" + "_" + boss_code + "_" + current_stage + "_";

        return monster_code;
    }

    private string Get_Monster_Stat_Code(bool is_boss)
    {
        int boss_code = is_boss ? 1 : 0;

        int current_stage = (Stage_Manager.instance.Get_Stage / 50) + 1;

        string monster_stat_code = "Monster_Stat" + "_" + boss_code + "_" + current_stage;

        return monster_stat_code;
    }

    #endregion
}
