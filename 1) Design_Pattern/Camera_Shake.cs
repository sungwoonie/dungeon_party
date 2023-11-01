using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Shake : SingleTon<Camera_Shake>
{
    public float shake_time;

    public void Shake_Camera(float amount)
    {
        StopAllCoroutines();
        StartCoroutine(Shake(amount));
    }

    private IEnumerator Shake(float amount)
    {
        Vector2 camera_position = transform.position;
        float timer = 0.0f;
        while (timer < shake_time)
        {
            float shake_position_x = (Random.insideUnitCircle * amount).x;
            float shake_position_y = (Random.insideUnitCircle * amount).y;
            Vector3 shake_position = new Vector3(shake_position_x, shake_position_y, -10);

            transform.position = shake_position;
            timer += Time.deltaTime;

            yield return null;
        }

        transform.position = new Vector3(camera_position.x, camera_position.y, -10);
    }
}
