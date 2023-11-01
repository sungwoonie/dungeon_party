using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Scroller : MonoBehaviour
{
    private Renderer[] back_grounds;

    private void Start()
    {
        back_grounds = GetComponentsInChildren<Renderer>();
    }

    private void OnEnable()
    {
        Event_Bus.Subscribe_Event(Event_Type.Ready, Start_Scroll);
        Event_Bus.Subscribe_Event(Event_Type.Combat, Stop_Scroll);
        Event_Bus.Subscribe_Event(Event_Type.Boss_Spawn, Stop_Scroll);
    }

    private void OnDisable()
    {
        Event_Bus.UnSubscribe_Event(Event_Type.Ready, Start_Scroll);
        Event_Bus.UnSubscribe_Event(Event_Type.Combat, Stop_Scroll);
        Event_Bus.UnSubscribe_Event(Event_Type.Boss_Spawn, Stop_Scroll);
    }

    private void Start_Scroll()
    {
        StartCoroutine(Scroll());
    }

    private IEnumerator Scroll()
    {
        while (true)
        {
            for (int i = 0; i < back_grounds.Length; i++)
            {
                Vector2 texture_offset = new Vector2(Time.deltaTime * 0.1f * (i+1),0 );
                back_grounds[i].material.mainTextureOffset += texture_offset;

                yield return null;
            }

            yield return null;
        }
    }

    private void Stop_Scroll()
    {
        StopAllCoroutines();
    }
}
