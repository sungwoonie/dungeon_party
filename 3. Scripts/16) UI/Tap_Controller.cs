using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tap_Controller : MonoBehaviour
{
    public bool play_on_start = true;
    private Tap_Button[] tap_buttons;

    private void Awake()
    {
        Initialize_Component();
    }

    private void Start()
    {
        if (play_on_start)
        {
            Deactive_Other_Taps(tap_buttons[0]);
        }
    }

    private void Initialize_Component()
    {
        tap_buttons = GetComponentsInChildren<Tap_Button>(true);
    }

    public void Deactive_Other_Taps(Tap_Button focus_tap)
    {
        foreach (Tap_Button tap_button in tap_buttons)
        {
            if (focus_tap != tap_button)
            {
                tap_button.Set_Tap(false);
            }
        }
    }
}