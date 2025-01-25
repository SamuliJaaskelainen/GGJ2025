using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static int highScore;

    public Transform sceneCamera;
    public float cameraDistance = 10.0f;
    public float maxCameraPredictionMultiplier = 1.0f;
    public float maxCameraPrediction = 1.0f;
    public float cameraSmoothness = 10.0f;
    public float currentScore;
    public TextMeshPro highScoreText;
    public Transform head;
    public bool isDead;
    public Animator anim;
    public float oxygen = 100.0f;
    public float automaticOxuygenLoss = 1.0f;
    public float force = 1000.0f;
    public float maxSpeed = 2.0f;
    public float turnSpeed = 100.0f;
    public float dashTime = 0.33f;
    public float dashForce = 4000.0f;
    public float dashMaxSpeed = 9.0f;
    public float dashOxygenConsumption = 10.0f;
    public float hurtTime = 0.5f;
    public float hurtForce = 1.0f;
    public GameObject[] oxygenLights;
    public Light headLight;
    public Color warningHeadlightColor;
    public AnimationCurve warningHeadlightAnim;
    public float warningHeadlightSpeed;
    public Transform bgDiv;
    public float bgDivDistance = 0.72f;

    InputAction moveAction;
    InputAction dashAction;
    Vector2 moveDir;
    Rigidbody rb;
    float dashTimer;
    bool isDashing;
    float hurtTimer;
    bool isHurt;
    Vector3 hurtDir;
    Color defaultHeadlightColor;
    float defaultHeadlightIntensity;
    float warningAnimTime;
    Vector2 moveInput;

    void Start()
    {
        highScoreText.text = "HIGH SCORE " + Mathf.Abs(highScore) + "m";
        moveAction = InputSystem.actions.FindAction("Move");
        dashAction = InputSystem.actions.FindAction("Dash");
        rb = GetComponent<Rigidbody>();
        defaultHeadlightColor = headLight.color;
        defaultHeadlightIntensity = headLight.intensity;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (isDead)
            return;

        moveInput = moveAction.ReadValue<Vector2>();

        if (dashAction.triggered && !isDashing && oxygen > dashOxygenConsumption * 1.1f)
        {
            oxygen -= dashOxygenConsumption;
            dashTimer = Time.time + dashTime;
            anim.SetTrigger("Dash");
        }
        isDashing = dashTimer > Time.time;

        isHurt = hurtTimer > Time.time;
        if(isHurt)
        {
            headLight.color = warningHeadlightColor;
            headLight.intensity = defaultHeadlightIntensity * warningHeadlightAnim.Evaluate(warningAnimTime);
            warningAnimTime += warningHeadlightSpeed * Time.deltaTime;
            if(warningAnimTime > 1.0f)
            {
                warningAnimTime -= 1.0f;
            }
        }
        else
        {
            headLight.color = defaultHeadlightColor;
            headLight.intensity = defaultHeadlightIntensity;
        }

        if(isDashing && Mathf.Abs(moveInput.y) < 0.3f)
        {
            moveDir = transform.up * Time.deltaTime * (isDashing ? dashForce : force);
        }
        else
        {
            if(Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                moveDir = (transform.up * 0.5f) * Time.deltaTime * (isDashing ? dashForce : force);
            }
            else
            {
                moveDir = (transform.up * -moveInput.y) * Time.deltaTime * (isDashing ? dashForce : force);
            }
        }
        
        oxygen -= moveInput.magnitude * Time.deltaTime;
        oxygen -= automaticOxuygenLoss * Time.deltaTime;

        if(oxygen < 0.0f)
        {
            if(currentScore < highScore)
            {
                highScore = Mathf.RoundToInt(currentScore);
            }

            isDead =  true;
            anim.SetBool("IsDead", true);
        }

        if (moveInput.y > 0.3f)
        {
            anim.SetFloat("Swim", -moveAction.ReadValue<Vector2>().magnitude);
        }
        else if (moveInput.y < -0.3f)
        {
            anim.SetFloat("Swim", moveAction.ReadValue<Vector2>().magnitude);
        }
        else if (moveInput.x > 0.3f)
        {
            anim.SetFloat("Swim", moveAction.ReadValue<Vector2>().magnitude);
        }
        else if (moveInput.x < -0.3f)
        {
            anim.SetFloat("Swim", moveAction.ReadValue<Vector2>().magnitude);
        }
        else
        {
            anim.SetFloat("Swim", 0.0f);
        }

        if (transform.position.y < currentScore)
        {
            currentScore = transform.position.y;
        }

        for(int i = 0; i < oxygenLights.Length; ++i)
        {
            oxygenLights[i].SetActive(oxygen >= (100.0f / oxygenLights.Length * i));
        }

        bgDiv.localPosition = new Vector3(0.0f, 0.0f, bgDivDistance);
        bgDiv.eulerAngles = new Vector3(270.0f, 0.0f, 0.0f);
    }

    void FixedUpdate()
    {
        if (isDead)
            return;

        if (isHurt)
        {
            moveDir = hurtDir * hurtForce;
        }

        Vector3 addForce = new Vector3(moveDir.x, moveDir.y, 0.0f);
        rb.AddForce(addForce, ForceMode.Acceleration);

        float currentMaxSpeed = isDashing ? dashMaxSpeed : maxSpeed;
        if (rb.linearVelocity.magnitude > currentMaxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * currentMaxSpeed;
        }

        Quaternion turn = Quaternion.identity;
        if(moveInput.x > 0.3f)
        {
            turn = Quaternion.Euler(new Vector3(0.0f, 0.0f, turnSpeed * Time.fixedDeltaTime * (isDashing ? 0.33f : 1.0f)));
        }
        else if(moveInput.x < -0.3f)
        {
            turn = Quaternion.Euler(new Vector3(0.0f, 0.0f, -turnSpeed * Time.fixedDeltaTime * (isDashing ? 0.33f : 1.0f)));
            
        }

        rb.MoveRotation(rb.rotation * turn);
    }

    void LateUpdate()
    {
        float y = rb.linearVelocity.y * maxCameraPredictionMultiplier;
        y = Mathf.Clamp(y, -maxCameraPrediction, 0.0f);
        y += transform.position.y;
        Vector3 target = new Vector3(0.0f, y, transform.position.z - cameraDistance);
        sceneCamera.transform.position = Vector3.Lerp(sceneCamera.transform.position, target, cameraSmoothness * Time.smoothDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Hurt")
        {
            hurtTimer = Time.time + hurtTime;
            hurtDir = (collision.GetContact(0).point - collision.transform.position).normalized;
            Debug.DrawLine(collision.GetContact(0).point, collision.GetContact(0).point + hurtDir, Color.white, 10.0f);
            anim.SetTrigger("Hurt");
        }
    }
}
