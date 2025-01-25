using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float health = 100.0f;
    public Rigidbody rb;
    public float force = 1.0f;
    public float wave = 1.0f;
    public float maxSpeed = 2.0f;
    Vector3 scaleTarget;

    float hitTimer;
    float hitDelay = 1.0f;
    Vector3 dir;
    float random;

    void Start()
    {
        random = Random.value;
        Invoke("SetConstraints", 0.1f);
    }

    void FixedUpdate()
    {
        if(Time.time > hitTimer)
        {
            dir = new Vector3(wave * Mathf.Sin(Time.time + random), 1.0f, 0.0f);
        }
        rb.AddForce(dir * force, ForceMode.Force);

        Debug.Log(dir);

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    void SetConstraints()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }

    void OnCollisionEnter(Collision collision)
    {
        health -= 30.0f;
        hitTimer = Time.time + hitDelay;
        dir = -dir;
        rb.linearVelocity = Vector3.zero;

        if (health < 0.0f)
        {
            Pop();
        }

        if(collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<Player>().oxygen = 100.0f;
        }
    }

    public void Pop()
    {
        Destroy(gameObject);
    }
}
