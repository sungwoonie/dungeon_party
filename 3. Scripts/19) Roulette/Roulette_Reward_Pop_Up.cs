using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roulette_Reward_Pop_Up : MonoBehaviour
{
    public Image reward_sprite;

    private Roulette_Content current_content;
    private GameObject pop_up;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        pop_up = transform.GetChild(0).gameObject;
    }

    #endregion

    #region "Set"

    public void Set_Reward_Pop_Up(Roulette_Content roulette_content)
    {
        Roulette_Controller.instance.rolling = false;

        current_content = roulette_content;
        reward_sprite.sprite = current_content.icon_image.sprite;

        pop_up.SetActive(true);
    }

    #endregion

    #region "Get Reward"

    public void Get_Reward()
    {
        Budget_Manager.instance.Earn_Budget(current_content.current_reward.reward_name, current_content.current_reward.reward_amount);

        current_content = null;

        pop_up.SetActive(false);
    }

    #endregion
}
