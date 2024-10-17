using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_Save : MonoBehaviour
{
    public float save_start_time;

    private GameObject pop_up;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    private void Start()
    {
        StartCoroutine(Check_Playing());
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        pop_up = transform.GetChild(0).gameObject;
    }

    #endregion

    #region "Check"

    private IEnumerator Check_Playing()
    {
        float current_time = 0.0f;

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                current_time = 0.0f;
            }
            else
            {
                current_time += Time.deltaTime;

                if (current_time > save_start_time)
                {
                    pop_up.SetActive(true);
                    StartCoroutine(Check_Save());
                    yield break;
                }
            }

            yield return null;
        }
    }

    private IEnumerator Check_Save()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pop_up.SetActive(false);
                StartCoroutine(Check_Playing());
                yield break;
            }

            yield return null;
        }
    }

    #endregion
}
