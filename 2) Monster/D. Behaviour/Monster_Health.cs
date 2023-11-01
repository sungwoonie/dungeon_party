using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Health : MonoBehaviour
{
    public double max_health;
    public double current_health;

    private SpriteRenderer sprite_renderer;
    private Monster_Behaviour behaviour;

    private void Awake()
    {
        behaviour = GetComponent<Monster_Behaviour>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Get_Damage(100);
        }
    }

    public void Initialize_Health(Monster_Stat stat)
    {
        current_health = stat.max_health;
        max_health = stat.max_health;
        Monster_Health_Bar.instance.Initialize_Health_Bar(max_health);
    }

    public void Get_Damage(double damage)
    {
        current_health -= damage;

        if (current_health <= 0)
        {
            behaviour.state_context.Transition(behaviour.death_state);
            Monster_Health_Bar.instance.Set_Health_Bar(max_health, 0);
        }
        else
        {
            StartCoroutine(Hit_Fade());
            Monster_Health_Bar.instance.Set_Health_Bar(max_health, current_health);
        }
    }

    private IEnumerator Hit_Fade()
    {
        Color hit_color = Color.black;
        Color basic_color = Color.white;

        for (int i = 0; i < 5; i++)
        {
            if (i % 2 == 0)
            {
                sprite_renderer.color = basic_color;
            }
            else
            {
                sprite_renderer.color = hit_color;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
