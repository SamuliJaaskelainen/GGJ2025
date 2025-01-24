using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float force = 1.0f;
    public float maxSpeed = 200.0f;

    InputAction moveAction;
    Vector2 moveDir;
    Rigidbody rb;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveDir = moveAction.ReadValue<Vector2>() * Time.deltaTime * force;
    }

    void FixedUpdate()
    {
        Vector3 addForce = new Vector3(moveDir.x, moveDir.y, 0.0f);
        rb.AddForce(addForce, ForceMode.Acceleration);

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }


}
