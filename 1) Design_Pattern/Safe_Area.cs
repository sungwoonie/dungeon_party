using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe_Area : MonoBehaviour
{
    private Vector2 minimum_anchor, maximum_anchor;

    private void Update()
    {
        Set_Safe_Area();
    }

    public void Set_Safe_Area()
    {
        var safe_area = GetComponent<RectTransform>();

        minimum_anchor = Screen.safeArea.min;
        minimum_anchor.x /= Screen.width;
        minimum_anchor.y /= Screen.height;

        maximum_anchor = Screen.safeArea.max;
        maximum_anchor.x /= Screen.width;
        maximum_anchor.y /= Screen.height;


        safe_area.anchorMin = minimum_anchor;
        safe_area.anchorMax = maximum_anchor;
    }
}
