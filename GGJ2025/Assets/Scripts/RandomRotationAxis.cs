using UnityEngine;

public class RandomRotationAxis : MonoBehaviour
{
    public Vector3 localAxis;

    void Start()
    {
        transform.Rotate(localAxis, Random.Range(0.0f, 360.0f),Space.Self);
    }
}
