using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Scroller : SingleTon<Map_Scroller>
{
    public float scroll_speed;

    public Map_Material[] materials;

    private MeshRenderer[] map_materials;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        Initialize_Event_Bus();
    }

    private void OnDisable()
    {
        Quit_Event_Bus();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        map_materials = GetComponentsInChildren<MeshRenderer>();
    }

    #endregion

    #region "Event Bus"

    private void Initialize_Event_Bus()
    {
        Event_Bus.Subscribe_Event(Game_State.Run, Start_Scroll);

        Event_Bus.Subscribe_Event(Game_State.Stop, Stop_Scroll);
        Event_Bus.Subscribe_Event(Game_State.Combat, Stop_Scroll);
        Event_Bus.Subscribe_Event(Game_State.All_Dead, Stop_Scroll);

        Event_Bus.Subscribe_Event(Game_State.Spawn_Boss, Stop_Scroll);
    }

    private void Quit_Event_Bus()
    {
        Event_Bus.UnSubscribe_Event(Game_State.Run, Start_Scroll);

        Event_Bus.UnSubscribe_Event(Game_State.Stop, Stop_Scroll);
        Event_Bus.UnSubscribe_Event(Game_State.Combat, Stop_Scroll);
        Event_Bus.UnSubscribe_Event(Game_State.All_Dead, Stop_Scroll);

        Event_Bus.UnSubscribe_Event(Game_State.Spawn_Boss, Stop_Scroll);
    }

    #endregion

    #region "Scroll"

    private void Start_Scroll()
    {
        Debug_Manager.Debug_In_Game_Message($"map scroll started");
        StartCoroutine(Scroll());
    }

    private IEnumerator Scroll()
    {
        while(true)
        {
            for (int i = 0; i < map_materials.Length; i++)
            {
                Vector2 offset = new Vector2(Time.deltaTime * Game_Time.game_time * (scroll_speed / (i + 1)), 0);
                map_materials[i].material.mainTextureOffset += offset;
            }

            yield return null;
        }
    }

    private void Stop_Scroll()
    {
        Debug_Manager.Debug_In_Game_Message($"map scroll stopped");
        StopAllCoroutines();
    }

    #endregion

    #region "Change Map"

    public void Change_Map(int floor)
    {
        Map_Material current_map_material = materials[floor % 5];

        for (int i = 0; i < map_materials.Length; i++)
        {
            if (current_map_material.materials.Length > i)
            {
                map_materials[i].material = current_map_material.materials[i];
            }
            else
            {
                map_materials[i].material = current_map_material.materials[current_map_material.materials.Length - 1];
            }
        }

        Camera.main.backgroundColor = current_map_material.back_color;
    }

    #endregion
}