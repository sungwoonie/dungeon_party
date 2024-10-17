using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Stage_Gage : SingleTon<Stage_Gage>
{
    public Image boss_spawn_icon;
    public Slider gage_slider;
    public TMP_Text gage_text;
    public Toggle auto_boss_toggle;

    private int current_gage;

    private bool auto_spawn_boss;

    #region "Unity"

    protected override void OnEnable()
    {
        base.OnEnable();

        Initialize_Event_Bus();
    }

    private void OnDisable()
    {
        Quit_Event_Bus();
    }

    protected override void Awake()
    {
        base.Awake();

        Initialize_Toggle();
    }

    private void Start()
    {
        auto_boss_toggle.isOn = Anti_Cheat_Manager.instance.Get("Auto_Boss", false);
        Auto_Boss();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Toggle()
    {
        auto_boss_toggle.onValueChanged.AddListener(delegate 
        {
            Auto_Boss();
        });
    }

    #endregion

    #region "Auto Boss"

    public void Auto_Boss()
    {
        auto_spawn_boss = auto_boss_toggle.isOn;
        Anti_Cheat_Manager.instance.Set("Auto_Boss", auto_spawn_boss);
    }

    #endregion

    #region "Event Bus"

    private void Initialize_Event_Bus()
    {
        Event_Bus.Subscribe_Event(Game_State.Spawn_Boss, Reset_Stage_Gage);
    }

    private void Quit_Event_Bus()
    {
        Event_Bus.UnSubscribe_Event(Game_State.Spawn_Boss, Reset_Stage_Gage);
    }

    #endregion

    #region "Gage"

    public void Set_Stage_Gage(int gage)
    {
        current_gage = gage;
        boss_spawn_icon.gameObject.SetActive(current_gage >= 100);
        Set_Slider(100, current_gage, false);
    }

    public void Reset_Stage_Gage()
    {
        current_gage = 0;
        boss_spawn_icon.gameObject.SetActive(false);
        Set_Slider(100, current_gage, false);

        Stage_Manager.instance.Save_Data();

        Debug_Manager.Debug_In_Game_Message("stage gage is reset");
    }

    public void Stage_Gage_Up()
    {
        int random_gage = Random.Range(10, 25);

        if (current_gage + random_gage >= 100)
        {
            Debug_Manager.Debug_In_Game_Message("stage gage is over 100");

            current_gage = 100;
            Set_Slider(100, current_gage, false);

            boss_spawn_icon.gameObject.SetActive(true);

            if (auto_spawn_boss)
            {
                Event_Bus.Publish(Game_State.Spawn_Boss);
            }
            else
            {
                Event_Bus.Publish(Game_State.Spawn_Normal);
            }
        }
        else
        {
            current_gage += random_gage;
            Set_Slider(100, current_gage, false);
            Event_Bus.Publish(Game_State.Spawn_Normal);

            Debug_Manager.Debug_In_Game_Message($"stage gage up to {current_gage}");
        }

        Stage_Manager.instance.Save_Data();
    }

    #endregion

    #region "Set"

    public void Set_Before(bool on_dungeon = false)
    {
        if (current_gage >= 100)
        {
            current_gage = 100;
            Set_Slider(100, current_gage, false);

            boss_spawn_icon.gameObject.SetActive(true);

            if (auto_spawn_boss)
            {
                Event_Bus.Publish(Game_State.Spawn_Boss);
            }
            else
            {
                if (on_dungeon == false)
                {
                    Event_Bus.Publish(Game_State.Spawn_Normal);
                }
            }
        }
        else
        {
            Set_Slider(100, current_gage, false);
        }
    }

    public void Set_Slider_To_Timer(float current_time, float max_time)
    {
        gage_text.text = $"{(int)current_time}";
        float lerp_amount = current_time / max_time;

        StopAllCoroutines();
        StartCoroutine(Slider_Lerp(lerp_amount));
    }

    public void Set_Slider(double max_value, double current_value, bool is_boss)
    {
        string gage_string = string.Empty;

        if (is_boss)
        {
            string max_value_string = Text_Change.ToCurrencyString(max_value);
            string current_value_string = Text_Change.ToCurrencyString(current_value);

            gage_string = $"{current_value_string} / {max_value_string}";
        }
        else
        {
            gage_string = $"{(current_value / max_value) * 100}%";
        }

        gage_text.text = gage_string;
        float lerp_amount = (float)(current_value / max_value);

        StopAllCoroutines();
        StartCoroutine(Slider_Lerp(lerp_amount));
    }

    private IEnumerator Slider_Lerp(float lerp_amount)
    {
        float current_time = 0.0f;

        while(gage_slider.value != lerp_amount)
        {
            current_time += Time.deltaTime;
            gage_slider.value = Mathf.Lerp(gage_slider.value, lerp_amount, (current_time / 2) * Game_Time.game_time);

            yield return null;
        }
    }

    #endregion

    #region "Get"

    public bool Can_Spawn_Boss()
    {
        if (current_gage >= 100)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int Get_Gage()
    {
        return current_gage;
    }

    #endregion
}