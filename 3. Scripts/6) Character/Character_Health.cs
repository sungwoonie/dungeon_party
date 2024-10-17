using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Character_Health : MonoBehaviour
{
    public Character_Health_Bar health_bar;

    private double[] health = { 0, 0 };
    private int class_type;

    private SpriteRenderer sprite_renderer;

    private Character_Controller character_controller;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    private void Start()
    {
        StartCoroutine(Cure_Per_Second());
        StartCoroutine(Check_Max_Health());
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
        character_controller = GetComponent<Character_Controller>();
    }

    #endregion

    #region "Set"

    public void Revive_Character_Health(Paid_Stat character)
    {
        class_type = character.class_type;

        health[0] = Stat_Manager.instance.Calculate_Stat(class_type, 2);
        health[1] = health[0];

        health_bar.Set_Health_bar(health[0], health[1], true);
        health_bar.Set_Health_Bar_Class_Type(character.class_type);

        Debug_Manager.Debug_In_Game_Message($"{character} health setted");
    }

    public void Set_Character_Health(Paid_Stat character, float ratio = 1.0f)
    {
        class_type = character.class_type;

        health[0] = Stat_Manager.instance.Calculate_Stat(class_type, 2);
        health[1] = health[0] * ratio;

        health_bar.Set_Health_bar(health[0], health[1]);
        health_bar.Set_Health_Bar_Class_Type(character.class_type);

        Debug_Manager.Debug_In_Game_Message($"{character} health setted");
    }

    private IEnumerator Check_Max_Health()
    {
        while (true)
        {
            if (health[0] != 0)
            {
                double current_max_health = Stat_Manager.instance.Calculate_Stat(class_type, 2);

                if (current_max_health != health[0])
                {
                    double remain_health = current_max_health - health[0];
                    health[0] = current_max_health;
                    health[1] += remain_health;

                    health_bar.Set_Health_bar(health[0], health[1]);
                }
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    #endregion

    #region "Cure"

    public void Cure(double amount)
    {
        if (health[0] != 0)
        {
            health[1] += amount;

            if (health[1] > health[0])
            {
                health[1] = health[0];
            }

            health_bar.Set_Health_bar(health[0], health[1]);
        }
    }

    private IEnumerator Cure_Per_Second()
    {
        float timer = 0.0f;

        while (true)
        {
            if (timer < 1)
            {
                timer += Time.deltaTime * Game_Time.game_time;
            }
            else
            {
                if (health[1] < health[0])
                {
                    if (!character_controller.Get_Dead())
                    {
                        timer = 0.0f;

                        Cure(Stat_Manager.instance.Calculate_Stat(class_type, 6));
                    }
                }
            }

            yield return null;
        }
    }

    #endregion

    #region "Get"

    public double[] Get_Current_Health()
    {
        return health;
    }

    #endregion

    #region "Get Damage"

    public bool Get_Damage(double damage)
    {
        Debug_Manager.Debug_In_Game_Message($"{gameObject.name} get {damage} damage");

        StartCoroutine(Blink_Sprite());

        health[1] -= damage;

        if (health[1] <= 0)
        {
            health[1] = 0;
            health_bar.Set_To_Revive_Bar();

            return true;
        }
        else
        {
            health_bar.Set_Health_bar(health[0], health[1]);

            return false;
        }
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

    #endregion
}