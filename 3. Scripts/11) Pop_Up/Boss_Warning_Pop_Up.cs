using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_Warning_Pop_Up : MonoBehaviour
{
    public Image glow;

    private GameObject pop_up;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    private void OnEnable()
    {
        Initialize_Event_Bus();
    }

    private void OnDisable()
    {
        Quit_Event_Bus();
    }

    #endregion

    #region "Event Bus"

    private void Initialize_Event_Bus()
    {
        Event_Bus.Subscribe_Event(Game_State.Spawn_Boss, Boss_Warning_On);
    }

    private void Quit_Event_Bus()
    {
        Event_Bus.UnSubscribe_Event(Game_State.Spawn_Boss, Boss_Warning_On);
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        pop_up = transform.GetChild(0).gameObject;
    }

    #endregion

    #region "Pop Up"

    public void Boss_Warning_On()
    {
        pop_up.SetActive(true);
        StartCoroutine(Glow_Fade());
    }

   private IEnumerator Glow_Fade()
    {
        float min = 0.3f;
        float max = 1.0f;

        Color fade_color = Color.white;

        while (Event_Bus.Get_Current_State() == Game_State.Spawn_Boss)
        {
            while (fade_color.a > min && Event_Bus.Get_Current_State() == Game_State.Spawn_Boss)
            {
                fade_color.a -= Time.deltaTime;
                glow.color = fade_color;

                yield return null;
            }

            while (fade_color.a < max && Event_Bus.Get_Current_State() == Game_State.Spawn_Boss)
            {
                fade_color.a += Time.deltaTime;
                glow.color = fade_color;

                yield return null;
            }

            yield return null;
        }

        glow.color = Color.white;
        pop_up.SetActive(false);
    }


    #endregion
}