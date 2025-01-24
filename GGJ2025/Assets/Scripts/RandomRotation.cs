using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    void Start()
    {
        transform.localRotation = Random.rotation;
    }
}
