using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : Observer
{
    public Paid_Stat current_class;
    
    private bool range_attack;
    private bool is_dead;
    private bool ready_to_combat;

    private Animator animator;
    private Object_Pooling attack_effect_pool;
    private Character_Health character_health;

    private Vector3 offset_position;

    private State_Context state_context;
    private IState run_state, combat_state, idle_state, boss_spawn_state, dead_state;

    private const string class_animator_path = "4. Class_Animator/";
    private const string class_stat_path = "1. Scriptable_Object/3) Class/";

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
        Initialize_State();
        Initialize_Range_Attack();
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
        attack_effect_pool = GetComponent<Object_Pooling>();
        offset_position = transform.position;
        character_health = GetComponent<Character_Health>();
    }

    private void Initialize_Observer()
    {
        Game_Time.Attach(this);
    }

    private void Initialize_State()
    {
        state_context = new State_Context(transform);

        run_state = gameObject.AddComponent<Character_Run_State>();
        combat_state = gameObject.AddComponent<Character_Combat_State>();
        idle_state = gameObject.AddComponent<Character_Idle_State>();
        boss_spawn_state = gameObject.AddComponent<Character_Boss_Spawn_State>();
        dead_state = gameObject.AddComponent<Character_Dead_State>();
    }

    private void Initialize_Range_Attack()
    {
        int current_class_type = current_class.class_type;

        switch (current_class_type)
        {
            case 2:
                range_attack = true;
                return;
            default:
                range_attack = false;
                break;
        }
    }

    #endregion

    #region "State"

    private void Set_State_Run()
    {
        state_context.Transition(run_state);
    }

    private void Set_State_Combat()
    {
        state_context.Transition(combat_state);
    }

    public void Set_State_Idle()
    {
        state_context.Transition(idle_state);
    }

    private void Set_Boss_Spawn_State()
    {
        state_context.Transition(boss_spawn_state);
    }

    private void Set_Death_State()
    {
        state_context.Transition(dead_state);
    }

    public void Set_State()
    {
        if (!is_dead)
        {
            Game_State current_game_state = Event_Bus.Get_Current_State();

            switch (current_game_state)
            {
                case Game_State.Run:
                    Set_State_Run();
                    break;
                case Game_State.Combat:
                    Set_State_Combat();
                    break;
                case Game_State.Stop:
                    Set_State_Idle();
                    break;
                case Game_State.Spawn_Boss:
                    Set_Boss_Spawn_State();
                    break;
                default:
                    break;
            }
        }
        else
        {
            state_context.Transition(dead_state);
        }
    }

    #endregion

    #region "Event Bus"

    private void Initialize_Event_Bus()
    {
        Event_Bus.Subscribe_Event(Game_State.Run, Set_State);
        Event_Bus.Subscribe_Event(Game_State.Combat, Set_State);
        Event_Bus.Subscribe_Event(Game_State.Stop, Set_State);
        Event_Bus.Subscribe_Event(Game_State.Spawn_Boss, Set_State);
    }

    private void Quit_Event_Bus()
    {
        Event_Bus.UnSubscribe_Event(Game_State.Run, Set_State);
        Event_Bus.UnSubscribe_Event(Game_State.Combat, Set_State);
        Event_Bus.UnSubscribe_Event(Game_State.Stop, Set_State);
        Event_Bus.UnSubscribe_Event(Game_State.Spawn_Boss, Set_State);
    }

    #endregion

    #region "Set"

    public void Set_Character_Class(string class_name)
    {
        float pre_health_ratio = 1.0f;

        if (is_dead)
        {
            pre_health_ratio = 0.0f;
        }
        else
        {

            if (character_health.Get_Current_Health()[0] != 0)
            {
                pre_health_ratio = (float)(character_health.Get_Current_Health()[1] / character_health.Get_Current_Health()[0]);
            }
        }

        RuntimeAnimatorController change_controller = Resources.Load<RuntimeAnimatorController>(class_animator_path + class_name);
        animator.runtimeAnimatorController = change_controller;
        animator.SetFloat("Game_Time", Game_Time.game_time);

        Paid_Stat target_stat = Resources.Load<Paid_Stat>(class_stat_path + class_name);
        current_class = target_stat;

        Stat_Manager.instance.class_stat_manager.Change_Class(current_class.class_type, current_class);

        character_health.Set_Character_Health(current_class, pre_health_ratio);

        Set_State();

        Debug_Manager.Debug_In_Game_Message($"{current_class.class_type} class setted to {class_name}");
    }

    public void Set_Ready_To_Combat(bool ready)
    {
        ready_to_combat = ready;
    }

    public void Set_Dead(bool dead)
    {
        is_dead = dead;
    }

    public void Set_To_Offset()
    {
        transform.position = offset_position;
        character_health.health_bar.Set_Health_Bar_Position(transform);
    }

    #endregion

    #region "Get"

    public Character_Health_Bar Get_Character_Health_Bar()
    {
        return character_health.health_bar;
    }

    public Vector3 Get_Offset_Position()
    {
        return offset_position;
    }

    public State_Context Get_Character_State_Context()
    {
        return state_context;
    }

    public Animator Get_Animator()
    {
        return animator;
    }

    public bool Get_Dead()
    {
        return is_dead;
    }

    public bool Get_Ready_To_Combat()
    {
        return ready_to_combat;
    }

    #endregion

    #region "Attack"

    public void Attack_Call_By_Animation_Event(int special)
    {
        if (!range_attack)
        {
            Give_Damage(special);
        }

        string attack_effect_name = current_class.name;

        Attack_Effect new_attack_effect = attack_effect_pool.Pool().GetComponent<Attack_Effect>();

        if (new_attack_effect != null)
        {
            new_attack_effect.Set_Attack_Effect(attack_effect_name, range_attack, special);
        }
    }

    public void Give_Damage(int special)
    {
        Monster_Controller current_monster = Monster_Spawner.instance.Get_Current_Monster();

        double attack_damage = 0.0f;
        int attack_count = 0;

        if (special == 0)
        {
            attack_damage = current_class.Get_Stat(21);
            attack_count = (int)current_class.Get_Stat(20);
        }
        else
        {
            attack_damage = current_class.Get_Stat(24);
            attack_count = (int)current_class.Get_Stat(23);
        }

        double damage = Stat_Manager.instance.Calculate_Stat(current_class.class_type, 0);
        double final_damage = damage * attack_damage * 0.01f;

        for (int i = 0; i < attack_count; i++)
        {
            bool critical = Stat_Manager.instance.Calculate_Critical(current_class.class_type, final_damage, out double critical_damage);
            final_damage = critical_damage;
            current_monster.Get_Damage(final_damage, critical);
        }

        Debug_Manager.Debug_In_Game_Message($"give {final_damage} damage to monster {attack_count} time");
    }

    #endregion

    #region "Get Damage"

    public void Get_Damage(double damage)
    {
        damage = damage - (damage * Stat_Manager.instance.Calculate_Stat(current_class.class_type, 7) * 0.01f);

        if (damage <= 0)
        {
            damage = 0;
        }

        is_dead = character_health.Get_Damage(damage);
        Damage_Text_Manager.instance.Set_Damage_Text(damage, is_dead, transform.position);

        StartCoroutine(Camera_Shake.Shake(0.02f));

        if (is_dead)
        {
            Set_Death_State();
            Debug_Manager.Debug_In_Game_Message($"{current_class} is dead");
        }
    }

    #endregion

    #region "Revive"

    public void Revive_Character()
    {
        is_dead = false;
        character_health.Revive_Character_Health(current_class);
        animator.SetBool("Death", false);

        Set_State();

        Debug_Manager.Debug_In_Game_Message($"{current_class.class_type} revived");
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