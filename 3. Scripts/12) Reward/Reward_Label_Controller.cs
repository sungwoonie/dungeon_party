using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward_Label_Controller : SingleTon<Reward_Label_Controller>
{
    private Reward_Label[] reward_labels;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        reward_labels = GetComponentsInChildren<Reward_Label>(true);
    }

    #endregion

    #region "Set"

    public void Set_Reward(string reward_name, string reward)
    {
        string reward_text = $"+ {reward} {Localization_Manager.instance.Get_Localized_String(reward_name)}";

        foreach (var reward_label in reward_labels)
        {
            if (reward_label.gameObject.activeSelf == false)
            {
                reward_label.Set_Label(reward_text);
                break;
            }
        }
    }

    #endregion
}