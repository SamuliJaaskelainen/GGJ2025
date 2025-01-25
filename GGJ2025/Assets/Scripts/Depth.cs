using UnityEngine;
using TMPro;

public class Depth : MonoBehaviour
{
    public TextMeshPro depthText;

    void Start()
    {
        depthText.text = Mathf.Abs(Mathf.RoundToInt(transform.position.y)) + "m";
    }
}
