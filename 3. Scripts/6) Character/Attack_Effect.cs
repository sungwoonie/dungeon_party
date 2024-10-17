using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack_Effect : Observer
{
    private Character_Controller character_controller;
    private Animator animator;
    private bool range_attack;

    private int special_attack;
    private readonly Vector3 spawn_position = new Vector3(0, 0.14f, 0);

    private const string resource_path = "6. Attack_Effect_Animator/";

    #region "Unity"

    private void Awake()
    {
        Initialize_Observer();
    }

    private void OnEnable()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        if (animator == null || character_controller == null)
        {
            character_controller = GetComponentInParent<Character_Controller>();
            animator = GetComponent<Animator>();
        }
    }

    private void Initialize_Observer()
    {
        Game_Time.Attach(this);
    }

    #endregion

    #region "Set"

    public void Set_Attack_Effect(string attack_effect_name, bool range_attack, int special)
    {
        this.range_attack = range_attack;
        special_attack = special;

        animator.runtimeAnimatorController = Get_Animator(attack_effect_name);

        if (range_attack)
        {
            transform.localPosition = spawn_position;
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }

        gameObject.SetActive(true);

        animator.SetFloat("Game_Time", Game_Time.game_time);
        animator.SetInteger("Special", special_attack);

        if (this.range_attack)
        {
            StartCoroutine(Move());
        }

        Debug_Manager.Debug_In_Game_Message($"{attack_effect_name} is setted and on");
    }

    #endregion

    #region "Move"

    private IEnumerator Move()
    {
        if (Monster_Spawner.instance.Get_Current_Monster() == null)
        {
            gameObject.SetActive(false);
            yield break;
        }

        Vector3 monster_position = Monster_Spawner.instance.Get_Current_Monster().transform.position;

        while (gameObject.activeSelf)
        {
            transform.Translate(Vector3.right * Time.deltaTime * 7.0f * Game_Time.game_time);
            yield return null;
        }
    }

    #endregion

    #region "Animation Call"

    public void Animation_End()
    {
        if (!range_attack)
        {
            gameObject.SetActive(false);
        }
    }

    #endregion

    #region "Get"

    private RuntimeAnimatorController Get_Animator(string animator_name)
    {
        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(resource_path + animator_name + "_Attack_Effect");
        return controller;
    }

    #endregion

    #region "Trigger"

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (range_attack)
        {
            if (collision.CompareTag("Monster"))
            {
                character_controller.Give_Damage(special_attack);

                gameObject.SetActive(false);

                Debug_Manager.Debug_In_Game_Message($"{character_controller.name}'s attack effect detected to monster");
            }
            else if (collision.CompareTag("Wall"))
            {
                gameObject.SetActive(false);
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