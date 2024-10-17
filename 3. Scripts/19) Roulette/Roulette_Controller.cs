using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Roulette_Controller : SingleTon<Roulette_Controller>
{
    public Transform rotate_roulette;

    private Roulette_Content[] roulette_contents;
    private Roulette_Data data;
    private Roulette_Reward[] current_rewards;
    private Roulette_Reward_Pop_Up reward_pop_up;

    public bool rolling;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    private void Start()
    {
        for (int i = 0; i < roulette_contents.Length; i++)
        {
            roulette_contents[i].Set_Content(current_rewards[i]);
        }
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        roulette_contents = GetComponentsInChildren<Roulette_Content>(true);
        reward_pop_up = GetComponentInChildren<Roulette_Reward_Pop_Up>(true);

        data = GetComponent<Roulette_Data>();
        current_rewards = data.Get_Reward();
    }

    #endregion

    #region "Roll"

    private void Start_Roll()
    {
        StartCoroutine(Roll_Roulette());
    }

    public void Roll()
    {
        if (rolling)
        {
            Error_Message.instance.Set_Error_Message("Error_Message_Rolling");
        }
        else
        {
            Reward_AD_Pop_Up.instance.Set_Reward_AD_Pop_Up("AD_Reward_Roulett", Start_Roll);
        }
    }

    private IEnumerator Roll_Roulette()
    {
        rolling = true;

        foreach (var roulette_content in roulette_contents)
        {
            roulette_content.Focus_Content(false);
        }

        float rotation_speed = 0.0f;
        float acceleration = 250.0f;
        float max_speed = 500.0f;

        int random_reward = Random.Range(0, roulette_contents.Length);

        while (rolling)
        {
            if (rotation_speed > max_speed)
            {
                max_speed = 0.0f;
                
                rotation_speed -= acceleration * Time.deltaTime;
            }
            else
            {
                rotation_speed += acceleration * Time.deltaTime;
            }

            if (rotation_speed < 10.0f && max_speed == 0.0f)
            {
                rolling = false;

                float current_rotation = rotate_roulette.eulerAngles.z;
                int index = (int)(current_rotation + 22.5f) / 45;
                roulette_contents[index].Focus_Content(true);
                reward_pop_up.Set_Reward_Pop_Up(roulette_contents[index]);
                yield break;
            }
            else
            {
                rotate_roulette.Rotate(Vector3.forward, rotation_speed * Time.deltaTime);
            }

            yield return null;
        }
    }

    #endregion

    #region "Set"

    public void Set_Off()
    {
        if (rolling)
        {
            Error_Message.instance.Set_Error_Message("Error_Message_Rolling");
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    #endregion
}