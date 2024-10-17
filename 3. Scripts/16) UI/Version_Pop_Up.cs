using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Version_Pop_Up : MonoBehaviour
{
    private GameObject pop_up;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    private void Start()
    {
        StartCoroutine(Check_Version());
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        pop_up = transform.GetChild(0).gameObject;
    }

    #endregion

    #region "Set"

    private void Set_Pop_Up(bool is_on)
    {
        pop_up.SetActive(is_on);
    }

    #endregion

    #region "Check"

    private IEnumerator Check_Version()
    {
        while (true)
        {
            if (Back_End_Controller.instance.Correct_Version() == false)
            {
                Set_Pop_Up(true);
            }
            else
            {
                if (pop_up.activeSelf)
                {
                    Set_Pop_Up(false);
                }
            }

            yield return new WaitForSeconds(60.0f);
        }
    }

    #endregion
}
