using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Text_Manager : SingleTon<Damage_Text_Manager>
{
    private Object_Pooling damage_text_pool;

    private readonly Vector3 offset_y = new Vector3(0, 0.3f, 0);

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        damage_text_pool = GetComponent<Object_Pooling>();
    }

    #endregion

    #region "Set Damage Text"

    public void Set_Damage_Text(double damage, bool critical, Vector3 target_position)
    {
        Damage_Text new_damage_text = damage_text_pool.Pool().GetComponent<Damage_Text>();
        new_damage_text.transform.position = Random_Position(target_position);
        new_damage_text.Set_Damage_Text(damage, critical);
    }

    private Vector3 Random_Position(Vector3 target_position)
    {
        Vector3 random_position = target_position + offset_y + (Random.insideUnitSphere * 0.3f);
        random_position.z = 0;

        return random_position;
    }

    #endregion
}