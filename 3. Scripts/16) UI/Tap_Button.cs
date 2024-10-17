using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Tap_Button : MonoBehaviour
{
    public GameObject[] focus_objects;
    public TMP_Text focus_text;
    public Color[] text_focus_color;

    public GameObject tap;
    public GameObject[] taps;

    public bool reset_scroll_rect = true;

    private Button button;
    private Tap_Controller tap_controller;
    private ScrollRect scroll_rect;

    private void Awake()
    {
        Initialize_Component();
    }

    private void Initialize_Component()
    {
        tap_controller = GetComponentInParent<Tap_Controller>();

        if (tap)
        {
            if (tap.GetComponentInChildren<ScrollRect>(true) != null)
            {
                scroll_rect = tap.GetComponentInChildren<ScrollRect>(true);
            }
        }
        
        button = GetComponent<Button>();
        button.onClick.AddListener(() => Set_Tap());
    }

    public void Set_Tap(bool focus = true)
    {
        if (scroll_rect)
        {
            if (reset_scroll_rect)
            {
                scroll_rect.content.anchoredPosition = new Vector2(scroll_rect.content.anchoredPosition.x, 0);
            }
        }

        if (tap)
        {
            tap.SetActive(focus);
        }

        foreach (var item in taps)
        {
            item.SetActive(focus);
        }

        foreach (GameObject focus_object in focus_objects)
        {
            focus_object.SetActive(focus);
        }

        if (focus_text)
        {
            if (focus)
            {
                focus_text.color = text_focus_color[0];
            }
            else
            {
                focus_text.color = text_focus_color[1];
            }
        }

        if (focus)
        {
            tap_controller.Deactive_Other_Taps(this);
        }
    }
}