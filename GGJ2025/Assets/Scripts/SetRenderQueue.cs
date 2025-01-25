using UnityEngine;

public class SetRenderQueue : MonoBehaviour
{
    public int renderQueue = 4000;

    void Start()
    {
        GetComponent<MeshRenderer>().material.renderQueue = renderQueue;
    }
}
