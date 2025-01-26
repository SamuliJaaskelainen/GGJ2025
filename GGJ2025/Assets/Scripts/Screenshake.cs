using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake : MonoBehaviour
{
    public static float intensity = 0.0f;
    public float speed = 10.0f;
    public float max = 1.0f;

    void Update()
    {
        intensity -= Time.unscaledDeltaTime;
        intensity = Mathf.Clamp(intensity, 0.0f, max);

        Quaternion targetRotation = Random.rotationUniform;
        targetRotation = Quaternion.Lerp(Quaternion.identity, targetRotation, intensity);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.unscaledDeltaTime);
    }
}
