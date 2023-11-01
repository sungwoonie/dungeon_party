using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stage_Manager : SingleTon<Stage_Manager>
{
    public TMP_Text stage_text;

    public int current_stage;

    private Stage_Reward reward;

    private void Start()
    {
        reward = GetComponent<Stage_Reward>();

        Set_Stage_Text();
    }

    public int Get_Stage
    {
        get
        {
            return current_stage;
        }
        set
        {
            current_stage = value;
        }
    }

    public void Stage_Up()
    {
        reward.Get_Reward(current_stage);

        current_stage++;

        Set_Stage_Text();

        if (current_stage % 50 == 0)
        {
            //fade
            //change monster and back ground
        }
    }

    public void Set_Stage_Text()
    {
        stage_text.text = "STAGE " + current_stage;
    }
}
