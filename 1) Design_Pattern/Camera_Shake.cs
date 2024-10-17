using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Camera_Shake
{
    public static bool is_shaking;
    public static bool can_shake = true;

    public static void Set_Shake(bool can)
    {
        can_shake = can;
    }

    public static IEnumerator Shake(float amount, float duration = 0.2f)
    {
        if (is_shaking || !can_shake)
        {
            yield break;
        }

        is_shaking = true;

        Vector2 camera_position = Camera.main.transform.position;
        float timer = 0.0f;
        while (timer < duration)
        {
            float shake_position_x = (Random.insideUnitCircle * amount).x;
            float shake_position_y = (Random.insideUnitCircle * amount).y;
            Vector3 shake_position = new Vector3(shake_position_x, shake_position_y, -10);

            Camera.main.transform.position = shake_position;
            timer += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.position = new Vector3(camera_position.x, camera_position.y, -10);

        is_shaking = false;
    }
}
