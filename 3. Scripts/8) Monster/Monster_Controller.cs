using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Controller : Observer
{
    private Monster_Stat current_stat;

    private Animator animator;
    private SpriteRenderer sprite_renderer;

    private State_Context state_context;
    private IState run_state, combat_state, idle_state;

    private readonly string animator_resources_path = "7. Monster_Animator/Floor_";

    private bool is_boss;
    private bool is_dead;

    private bool is_dungeon_monster;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
        Initialize_State();
        Initialize_Observer();
    }

    private void OnEnable()
    {
        Initialize_Event_Bus();
    }

    private void OnDisable()
    {
        Quit_Event_Bus();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        animator = GetComponent<Animator>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    private void Initialize_State()
    {
        state_context = new State_Context(transform);

        run_state = gameObject.AddComponent<Monster_Run_State>();
        combat_state = gameObject.AddComponent<Monster_Combat_State>();
        idle_state = gameObject.AddComponent<Monster_Idle_State>();
    }

    private void Initialize_Observer()
    {
        Game_Time.Attach(this);
    }

    #endregion

    #region "State"

    public void Set_State_Run()
    {
        if (!is_boss)
        {
            state_context.Transition(run_state);
        }
    }

    public void Set_State_Combat()
    {
        state_context.Transition(combat_state);
    }

    public void Set_Idle_State()
    {
        state_context.Transition(idle_state);
    }

    #endregion

    #region "Event Bus"

    private void Initialize_Event_Bus()
    {
        Event_Bus.Subscribe_Event(Game_State.Run, Set_State_Run);
        Event_Bus.Subscribe_Event(Game_State.Combat, Set_State_Combat);
        Event_Bus.Subscribe_Event(Game_State.Stop, Set_Idle_State);

        Event_Bus.Subscribe_Event(Game_State.All_Dead, Set_Idle_State);
    }

    private void Quit_Event_Bus()
    {
        Event_Bus.UnSubscribe_Event(Game_State.Run, Set_State_Run);
        Event_Bus.UnSubscribe_Event(Game_State.Combat, Set_State_Combat);
        Event_Bus.UnSubscribe_Event(Game_State.Stop, Set_Idle_State);

        Event_Bus.UnSubscribe_Event(Game_State.All_Dead, Set_Idle_State);
    }

    #endregion

    #region "Set"

    public void Set_Monster_Controller(Monster_Stat target_stat, bool is_boss, bool dungeon = false, Dungeon_Type dungeon_type = Dungeon_Type.Gold)
    {
        current_stat = target_stat;

        this.is_boss = is_boss;
        is_dead = false;

        is_dungeon_monster = dungeon;

        Set_Animator(Convert.ToInt32(is_boss), (int)dungeon_type);

        if (is_boss)
        {
            Stage_Gage.instance.Set_Slider(current_stat.max_health, current_stat.current_health, is_boss);
        }

        sprite_renderer.color = Color.white;

        gameObject.SetActive(true);
        animator.SetFloat("Game_Time", Game_Time.game_time);
    }

    private void Set_Animator(int is_boss, int dungeon_type = 0)
    {
        
        int current_floor = is_dungeon_monster ? dungeon_type + 1 : (Stage_Manager.instance.Get_Current_Stage()[0] - 1) % 5 + 1;
        int monster_type = is_boss == 1 ? 0 : UnityEngine.Random.Range(0, 4);
        string animator_name = $"Monster_{current_floor}_{is_boss}_{monster_type}"; //last monster number have to change to random number that 0 to 5
        
        RuntimeAnimatorController runtime_animator_controller = Resources.Load<RuntimeAnimatorController>($"{animator_resources_path}{current_floor}/{animator_name}");
        animator.runtimeAnimatorController = runtime_animator_controller;
    }

    #endregion

    #region "Get"

    public Monster_Stat Get_Monster_Stat()
    {
        return current_stat;
    }

    public State_Context Get_Monster_State_Context()
    {
        return state_context;
    }

    public Animator Get_Animator()
    {
        return animator;
    }

    public bool Is_Dead()
    {
        return is_dead;
    }

    #endregion

    #region "Animation"

    public void Boss_Intro_End()
    {
        Event_Bus.Publish(Game_State.Combat);
    }

    #endregion

    #region "Combat"

    public void Give_Damage()
    {
        Character_Manager.instance.Give_Damage_To_Alive_Character(current_stat.damage);

        Debug_Manager.Debug_In_Game_Message($"{current_stat} monster give {current_stat.damage} damage");
    }

    public void Get_Damage(double damage, bool critical)
    {
        if (damage >= double.MaxValue)
        {
            damage = double.MaxValue;
        }

        Damage_Text_Manager.instance.Set_Damage_Text(damage, critical, transform.position);

        if (is_dead || Event_Bus.Get_Current_State() != Game_State.Combat)
        {
            return;
        }

        StartCoroutine(Blink_Sprite());

        current_stat.current_health -= damage;

        Debug_Manager.Debug_In_Game_Message($"{current_stat} monster got {damage} damage");

        if(current_stat.current_health <= 0)
        {
            Monster_Dead();
        }

        Set_Health_Bar();
    }

    private IEnumerator Blink_Sprite()
    {
        Color blink_color = Color.red;
        Color offset_color = Color.white;

        for (int i = 0; i < 3; i++)
        {
            if (i % 2 == 0)
            {
                sprite_renderer.color = blink_color;
            }
            else
            {
                sprite_renderer.color = offset_color;
            }

            yield return new WaitForSeconds(0.1f);
        }

        sprite_renderer.color = offset_color;

        yield return null;
    }

    private void Set_Health_Bar()
    {
        if (is_boss)
        {
            Stage_Gage.instance.Set_Slider(current_stat.max_health, current_stat.current_health, is_boss);
        }
        else
        {
            Monster_Health_Bar.instance.Set_Health_Bar(current_stat.max_health, current_stat.current_health);
        }
    }

    private void Monster_Dead()
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Death");
        animator.SetTrigger("Death");

        animator.Play("Death", 0);

        is_dead = true;

        current_stat.current_health = 0;
        Event_Bus.Publish(Game_State.Stop);

        if (is_dungeon_monster)
        {
            Dungeon_Manager.instance.Kill_Count_Up();
        }
        else
        {
            if (is_boss)
            {
                Battle_Pass_Manager.instance.Get_Requirement("boss_clear", 1);
            }
            else
            {
                Battle_Pass_Manager.instance.Get_Requirement("kill_count", 1);
                Quest_Manager.instance.Increase_Requirement("kill_count", 1);
            }

        }
        Debug_Manager.Debug_In_Game_Message($"{current_stat} monster is dead");
    }

    public void Monster_Dead_Animation_End()
    {
        gameObject.SetActive(false);

        if (is_boss)
        {
            Stage_Manager.instance.Stage_Up();
            Stage_Gage.instance.Reset_Stage_Gage();
        }
        else
        {
            if (is_dungeon_monster)
            {
                if (Dungeon_Manager.instance.In_Dungeon())
                {
                    Event_Bus.Publish(Game_State.Spawn_Dungeon);
                }
            }
            else
            {
                Stage_Manager.instance.Stage_Gage_Up();
            }
        }
    }

    #endregion

    #region "Observer"

    public override void Notify()
    {
        if (gameObject.activeSelf)
        {
            animator.SetFloat("Game_Time", Game_Time.game_time);
        }
    }

    #endregion
}