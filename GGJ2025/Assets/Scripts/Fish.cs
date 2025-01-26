using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    public Transform path;
    public float speed = 1.0f;
    public float wave = 1.0f;

    List<Vector3> pathNodes = new List<Vector3>();
    int targetNode = 0;
    float random;
    bool playedSound = false;

    void Start()
    {
        if(path == null)
        {
            Debug.LogError("No path!", gameObject);
            enabled = false;
            return;
        }

        for(int i = 0; i < path.childCount; ++i)
        {
            pathNodes.Add(path.GetChild(i).transform.position);
            if(i > 0)
            { 
                Debug.DrawLine(pathNodes[i - 1], pathNodes[i], Color.red, 30.0f);
            }
        }
        random = Random.value;
    }

    void Update()
    {
        if(meshRenderer.isVisible)
        {
            if (!playedSound)
            {
                // TODO: Play appear sound
                playedSound = true;
            }

            Vector3 target = pathNodes[targetNode] + (Vector3.up * wave * Mathf.Sin(Time.time + random));
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        
            Quaternion targetRot = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.3f)
            {
                targetNode++;
                if(targetNode >= pathNodes.Count)
                {
                    targetNode = 0;
                }
            }
        }
    }
}
