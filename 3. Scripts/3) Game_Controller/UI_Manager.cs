using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : SingleTon<UI_Manager>
{
    private Paid_Stat_Manager[] paid_stat_managers;
    private Upgradable_Stat_Manager[] upgradable_stat_managers;

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
        paid_stat_managers = GetComponentsInChildren<Paid_Stat_Manager>(true);
        upgradable_stat_managers = GetComponentsInChildren<Upgradable_Stat_Manager>(true);
    }

    #endregion

    #region "Pop Up"

    public void Set_Upgradable_Content(Upgradable_Stat_Manager target)
    {
        foreach (var upgradable_stat_manager in upgradable_stat_managers)
        {
            if (upgradable_stat_manager == target)
            {
                upgradable_stat_manager.Set_Pop_Up();
                Debug_Manager.Debug_In_Game_Message($"set {upgradable_stat_manager.name} pop up");
            }
        }
    }

    public void Set_Paid_Content(Paid_Stat_Manager target)
    {
        foreach (var paid_stat_manager in paid_stat_managers)
        {
            if (paid_stat_manager == target)
            {
                paid_stat_manager.Set_Pop_Up();
                Debug_Manager.Debug_In_Game_Message($"set {paid_stat_manager.name} pop up");
            }
        }
    }

    #endregion
}