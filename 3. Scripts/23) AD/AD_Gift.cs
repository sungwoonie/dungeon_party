using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AD_Gift : MonoBehaviour
{
    public float move_speed;
    private Vector2 appear_offset_position = new Vector2(10, 1);

    #region "Set"

    public void Set_AD_Gift()
    {
        if (gameObject.activeSelf)
        {
            return;
        }

        transform.position = appear_offset_position;
        gameObject.SetActive(true);

        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float x;
        float y;
        Vector2 move_position;

        while (transform.position.x > -9.0f)
        {
            x = Mathf.Lerp(transform.position.x, -10.0f, Time.deltaTime * move_speed);
            y = Mathf.Sin(Time.time * 2f) * 0.1f;

            move_position.x = x;
            move_position.y = y;

            transform.position = move_position;

            yield return null;
        }

        gameObject.SetActive(false);
    }

    #endregion

    #region "On Click"

    private void OnMouseUpAsButton()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            Reward_AD_Pop_Up.instance.Set_Reward_AD_Pop_Up("AD_Reward_AD_Gift", Give_Reward);
            gameObject.SetActive(false);
        }
    }

    #endregion

    #region "Reward"

    private void Give_Reward()
    {
        Budget_Manager.instance.Get_Paid_Diamond(200.0f);
    }

    #endregion
}
