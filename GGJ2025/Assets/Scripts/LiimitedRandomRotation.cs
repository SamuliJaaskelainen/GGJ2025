using UnityEngine;

public class LiimitedRandomRotation : MonoBehaviour
{
    public Vector3 localAxis;
    public float min = -10.0f;
    public float max = 10.0f;

    void Start()
    {
        transform.Rotate(localAxis, Random.Range(min, max), Space.Self);
    }
}
