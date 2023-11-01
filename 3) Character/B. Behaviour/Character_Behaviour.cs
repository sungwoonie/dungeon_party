using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Behaviour : MonoBehaviour
{
    public Class_Stat current_class;

    private Animator animator;

    public State_Context state_context;
    public IState ready_state, combat_state, boss_spawn;

    private void OnEnable()
    {
        Event_Bus.Subscribe_Event(Event_Type.Ready, Ready);
        Event_Bus.Subscribe_Event(Event_Type.Combat, Combat);
        Event_Bus.Subscribe_Event(Event_Type.Boss_Spawn, Spawn_Boss);

    }

    private void OnDisable()
    {
        Event_Bus.UnSubscribe_Event(Event_Type.Ready, Ready);
        Event_Bus.UnSubscribe_Event(Event_Type.Combat, Combat);
        Event_Bus.UnSubscribe_Event(Event_Type.Boss_Spawn, Spawn_Boss);
    }

    private void Initialize_State()
    {
        state_context = new State_Context(transform);
        ready_state = gameObject.AddComponent<Ready_State_Character>();
        combat_state = gameObject.AddComponent<Combat_State_Character>();
        boss_spawn = gameObject.AddComponent<Boss_Spawn_State_Character>();
    }

    private void Awake()
    {
        Initialize_State();

        animator = GetComponent<Animator>();
    }

    public void Change_Class(Class_Stat target_class)
    {
        current_class = target_class;
        animator.runtimeAnimatorController = current_class.animator;
    }

    public void Ready()
    {
        state_context.Transition(ready_state);
    }

    public void Combat()
    {
        state_context.Transition(combat_state);
    }

    public void Spawn_Boss()
    {
        state_context.Transition(boss_spawn);
    }

    public void Give_Damage()
    {
        Monster_Behaviour current_monster = Monster_Spawner.instance.Get_Current_Monster().GetComponent<Monster_Behaviour>();

        if (current_monster.health.current_health > 0 && state_context.Current_State.Equals(combat_state))
        {
            current_monster.health.Get_Damage(10);
        }
    }
}
