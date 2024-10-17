using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Spawner : SingleTon<Monster_Spawner>
{
    public Vector3 spawn_position;
    public Vector3 boss_spawn_position;
    public Vector3 combat_position;

    private Monster_Stat_Manager monster_stat_manager;
    private Object_Pooling monster_pool;

    private Monster_Controller current_spawned_monster;

    #region "Unity"

    protected override void OnEnable()
    {
        base.OnEnable();

        Initialize_Event_Bus();
    }

    private void OnDisable()
    {
        Quit_Event_Bus();
    }

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        monster_pool = GetComponent<Object_Pooling>();
        monster_stat_manager = GetComponent<Monster_Stat_Manager>();
    }

    #endregion

    #region "Event Bus"

    private void Initialize_Event_Bus()
    {
        Event_Bus.Subscribe_Event(Game_State.Spawn_Normal, Spawn_Normal_Monster);
        Event_Bus.Subscribe_Event(Game_State.Spawn_Dungeon, Spawn_Dungeon_Monster);

        Event_Bus.Subscribe_Event(Game_State.Spawn_Boss, Spawn_Boss_Monster);
    }

    private void Quit_Event_Bus()
    {
        Event_Bus.UnSubscribe_Event(Game_State.Spawn_Normal, Spawn_Normal_Monster);
        Event_Bus.UnSubscribe_Event(Game_State.Spawn_Dungeon, Spawn_Dungeon_Monster);

        Event_Bus.UnSubscribe_Event(Game_State.Spawn_Boss, Spawn_Boss_Monster);
    }

    #endregion

    #region "Spawn"

    public void Spawn_Boss_Button()
    {
        if (Stage_Manager.instance.Can_Spawn_Boss())
        {
            Event_Bus.Publish(Game_State.Spawn_Boss);
        }
        else
        {
            return;
        }
    }

    public void Spawn_Normal_Monster()
    {
        Debug_Manager.Debug_In_Game_Message($"normal monster spawn start");
        Spawn_Monster(false);
    }

    public void Spawn_Boss_Monster()
    {
        Debug_Manager.Debug_In_Game_Message($"boss monster spawn start");
        Spawn_Monster(true);
    }

    public void Spawn_Dungeon_Monster()
    {
        current_spawned_monster = null;

        Remove_All_Spawned_Monsters();

        Monster_Stat new_monster_stat = monster_stat_manager.Get_Dungeon_Monster_Stat();
        Monster_Controller new_monster = monster_pool.Pool().GetComponent<Monster_Controller>();
        new_monster.transform.position = spawn_position;

        new_monster.Set_Monster_Controller(new_monster_stat, false, true);

        current_spawned_monster = new_monster;

        Monster_Health_Bar.instance.Set_Monster_Health(new_monster);

        Event_Bus.Publish(Game_State.Run);

        Debug_Manager.Debug_In_Game_Message($"dungeon {new_monster.Get_Monster_Stat()} is spawned");
    }

    public void Spawn_Monster(bool is_boss)
    {
        if (Dungeon_Manager.instance.In_Dungeon())
        {
            return;
        }

        current_spawned_monster = null;

        Remove_All_Spawned_Monsters();

        Monster_Stat new_monster_stat = monster_stat_manager.Get_Monster_Stat_By_Current_Stage(is_boss);
        Monster_Controller new_monster = monster_pool.Pool().GetComponent<Monster_Controller>();
        new_monster.transform.position = is_boss ? boss_spawn_position : spawn_position;

        new_monster.Set_Monster_Controller(new_monster_stat, is_boss);

        current_spawned_monster = new_monster;

        if (!is_boss)
        {
            Event_Bus.Publish(Game_State.Run);
            Monster_Health_Bar.instance.Set_Monster_Health(new_monster);
        }
        else
        {
            Quest_Manager.instance.Increase_Requirement("spawn_boss", 1);
            Monster_Health_Bar.instance.gameObject.SetActive(false);
        }

        Debug_Manager.Debug_In_Game_Message($"{new_monster.Get_Monster_Stat()} is spawned");
    }

    #endregion

    #region "Set"

    public void Remove_All_Spawned_Monsters()
    {
        monster_pool.Set_All_Pool_To_Off();
        Monster_Health_Bar.instance.gameObject.SetActive(false);
    }

    #endregion

    #region "Set Spawner"

    public void Monster_Dead()
    {
    }

    #endregion

    #region "Get"

    public Monster_Controller Get_Current_Monster()
    {
        return current_spawned_monster;
    }

    public Vector3 Get_Combat_Position()
    {
        return combat_position;
    }

    #endregion
}