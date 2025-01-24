using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float currentScore;
    public bool isDead;
    public float oxygen = 100.0f;
    public float automaticOxuygenLoss = 1.0f;
    public float force = 1000.0f;
    public float maxSpeed = 2.0f;
    public float dashTime = 0.33f;
    public float dashForce = 4000.0f;
    public float dashMaxSpeed = 9.0f;

    InputAction moveAction;
    InputAction dashAction;
    Vector2 moveDir;
    Rigidbody rb;
    float dashTimer;
    bool isDashing;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        dashAction = InputSystem.actions.FindAction("Dash");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(isDead)
            return;

        if (dashAction.triggered && !isDashing)
        {
            Debug.Log("Dash!");
            oxygen -= 10.0f;
            dashTimer = Time.time + dashTime;
        }
        isDashing = dashTimer > Time.time;

        moveDir = moveAction.ReadValue<Vector2>() * Time.deltaTime * (isDashing ? dashForce : force);
        oxygen -= moveAction.ReadValue<Vector2>().magnitude * Time.deltaTime;

        oxygen -= automaticOxuygenLoss * Time.deltaTime;

        if(oxygen < 0.0f)
        {
            isDead =  true;
        }

        if(transform.position.y < currentScore)
        {
            currentScore = transform.position.y;
        }
    }

    void FixedUpdate()
    {
        if (isDead)
            return;

        Vector3 addForce = new Vector3(moveDir.x, moveDir.y, 0.0f);
        rb.AddForce(addForce, ForceMode.Acceleration);

        float currentMaxSpeed = isDashing ? dashMaxSpeed : maxSpeed;
        if (rb.linearVelocity.magnitude > currentMaxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * currentMaxSpeed;
        }
    }
}
