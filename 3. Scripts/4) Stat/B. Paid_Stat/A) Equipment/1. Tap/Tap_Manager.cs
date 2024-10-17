using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tap_Manager : MonoBehaviour
{
    public GameObject[] class_taps;
    public Tap[] equipment_taps;

    private Equipment_Stat_Manager stat_manager;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        stat_manager = GetComponentInParent<Equipment_Stat_Manager>();
    }

    #endregion

    #region "Set"

    public void Set_Class_Tap(int class_type)
    {
        if (stat_manager == null)
        {
            Initialize_Component();
        }

        for (int i = 0; i < class_taps.Length; i++)
        {
            class_taps[i].SetActive(i == class_type);
        }

        stat_manager.Set_Slots_By_Class(class_type);
    }

    public void Set_Equipment_Tap(int equipment_type)
    {
        int current_class_type = 0;

        for (int i = 0; i < class_taps.Length; i++)
        {
            if (class_taps[i].activeSelf)
            {
                current_class_type = i;
                break;
            }
        }

        for (int i = 0; i < equipment_taps[current_class_type].taps.Length; i++)
        {
            equipment_taps[current_class_type].taps[i].SetActive(i == equipment_type);
        }
    }

    #endregion
}


#region "Tap Struct"

[System.Serializable]
public struct Tap
{
    public GameObject[] taps;
}

#endregion