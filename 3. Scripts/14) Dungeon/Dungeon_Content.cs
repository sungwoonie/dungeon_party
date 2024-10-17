using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Content : MonoBehaviour
{
    public Dungeon_Type dungeon_type;

    public void On_Click_Dungeon_Content()
    {
        Dungeon_Detail_Pop_Up.instance.Set_Dungeon_Detail_Pop_Up(dungeon_type);
    }
}