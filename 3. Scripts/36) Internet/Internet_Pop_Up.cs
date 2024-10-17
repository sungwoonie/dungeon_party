using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Internet_Pop_Up : SingleTon<Internet_Pop_Up>
{
    private GameObject pop_up;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    private void Start()
    {
        StartCoroutine(Check_Internet());
    }

    #endregion

    #region "Get"

    public bool Pop_Up_Activating()
    {
        return pop_up.activeSelf;
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        pop_up = transform.GetChild(0).gameObject;
    }

    #endregion

    #region "Check"

    private IEnumerator Check_Internet()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                if (!pop_up.activeSelf)
                {
                    pop_up.SetActive(true);
                }
            }
            else
            {
                if (pop_up.activeSelf)
                {
                    pop_up.SetActive(false);
                }
            }
        }
    }

    #endregion

    #region "Event"

    public IEnumerator Wait_For_Internet(UnityAction internet_event)
    {
        yield return new WaitUntil(() => Pop_Up_Activating() == false);

        internet_event.Invoke();
    }

    #endregion
}