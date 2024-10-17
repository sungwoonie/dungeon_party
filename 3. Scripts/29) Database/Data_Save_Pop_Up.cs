using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Save_Pop_Up : SingleTon<Data_Save_Pop_Up>
{
    private GameObject pop_up;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        pop_up = transform.GetChild(0).gameObject;
    }

    #endregion

    #region "Set"

    public void Set_Pop_Up(bool is_on)
    {
        pop_up.SetActive(is_on);
    }

    #endregion
}