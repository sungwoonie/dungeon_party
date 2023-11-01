using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Behaviour : MonoBehaviour
{
    public Monster_Stat stat;
    public bool is_boss;

    [HideInInspector] public Monster_Health health;
    [HideInInspector] public State_Context state_context;
    [HideInInspector] public IState ready_state, combat_state, death_state;

    #region "Event Bus"

    private void OnEnable()
    {
        Event_Bus.Subscribe_Event(Event_Type.Combat, Combat);
    }

    private void OnDisable()
    {
        Event_Bus.UnSubscribe_Event(Event_Type.Combat, Combat);
    }

    #endregion

    #region "State"

    private void Initialize_State_Pattern()
    {
        state_context = new State_Context(transform);
        ready_state = gameObject.AddComponent<Ready_State_Monster>();
        combat_state = gameObject.AddComponent<Combat_State_Monster>();
        death_state = gameObject.AddComponent<Death_State_Monster>();
    }

    public void Combat()
    {
        state_context.Transition(combat_state);
    }

    #endregion

    private void Awake()
    {
        health = GetComponent<Monster_Health>();

        Initialize_State_Pattern();
    }

    public void Initialize_Monster(Monster_Stat target_stat)
    {
        stat = target_stat;
        health.Initialize_Health(target_stat);

        state_context.Transition(ready_state);
    }

    public void Give_Damage()
    {
        Character_Health.instance.Get_Damage(stat.damage);
    }
}
