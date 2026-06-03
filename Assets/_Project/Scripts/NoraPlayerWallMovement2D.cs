using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class NoraPlayerWallMovement2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] [Tooltip("Gravity manager that defines the active gravity direction and zone-based gravity vector.")]
    private NoraGravityFieldManager gravityManager;

    [SerializeField] [Tooltip("Layer mask used to detect grounded surfaces relative to active gravity.")]
    private LayerMask groundLayer;

    [SerializeField] [Tooltip("Player Rigidbody2D used for movement and force application.")]
    private Rigidbody2D rb;

    [Header("Surface Movement")]
    [SerializeField] [Tooltip("Base movement speed along the current gravity-aligned surface tangent.")]
    [Min(0f)]
    private float moveSpeed = 8f;

    [SerializeField] [Tooltip("How quickly the player reaches the target sideways speed while using keyboard input.")]
    [Min(0f)]
    private float moveAcceleration = 35f;

    [Header("Gravity / Stickiness")]
    [SerializeField] [Tooltip("Fallback gravity strength used only if the manager does not return a valid gravity vector.")]
    [Min(0f)]
    private float gravityStrength = 25f;

    [SerializeField] [Tooltip("Extra force that keeps the player attached to the current surface while grounded.")]
    [Min(0f)]
    private float stickForce = 20f;

    [SerializeField] [Tooltip("Distance of the raycast used to detect if the player is grounded.")]
    [Min(0.01f)]
    private float groundCheckDistance = 0.6f;

    [Header("Jetpack")]
    [SerializeField] [Tooltip("Key used to activate the jetpack. Default is Left Shift.")]
    private KeyCode jetpackKey = KeyCode.LeftShift;

    [SerializeField] [Tooltip("Instant burst applied once when the jetpack key is first pressed.")]
    [Min(0f)]
    private float jetpackBurstImpulse = 10f;

    [SerializeField] [Tooltip("Continuous lift force applied while the jetpack key is held after the initial burst.")]
    [Min(0f)]
    private float jetpackSustainForce = 8f;

    [SerializeField] [Tooltip("Maximum duration in seconds that continuous jetpack sustain can run during a single hold.")]
    [Min(0f)]
    private float maxJetpackHoldDuration = 0.6f;

    [SerializeField] [Tooltip("Fuel consumed immediately when the burst begins.")]
    [Min(0f)]
    private float fuelCostPerBurst = 10f;

    [SerializeField] [Tooltip("Fuel consumed per second while the jetpack key is held for sustained lift.")]
    [Min(0f)]
    private float fuelCostPerSecondWhileHolding = 20f;

    [SerializeField] [Tooltip("Maximum fuel amount available to the player.")]
    [Min(0f)]
    private float maxFuel = 100f;

    [SerializeField] [Tooltip("Current fuel amount. Visible for tuning and play mode observation.")]
    [Min(0f)]
    private float currentFuel = 100f;

    [SerializeField] [Tooltip("If enabled, fuel is reset to full when the object awakens.")]
    private bool startWithFullFuel = true;

    [Header("Jetpack Steering")]
    [SerializeField] [Tooltip("Default steering influence while using the jetpack. This is the medium steering model and should be the normal tuning path.")]
    [Range(0f, 1f)]
    private float jetpackSteeringInfluence = 0.5f;

    [SerializeField] [Tooltip("If enabled, use the strong steering model override instead of the default medium steering model.")]
    private bool useStrongSteeringOverride = false;

    [SerializeField] [Tooltip("Steering influence used when the strong steering override is enabled.")]
    [Range(0f, 1f)]
    private float strongSteeringInfluence = 0.85f;

    [SerializeField] [Tooltip("If enabled, vertical input is also considered when steering the jetpack. W pushes more away from gravity, S reduces or redirects thrust.")]
    private bool useVerticalInputForJetpackSteering = true;

    [Header("Rotation")]
    [SerializeField] [Tooltip("Sprite rotation offset in degrees to align artwork to the gravity direction.")]
    [Range(-360f, 360f)]
    private float spriteUpOffset = 0f;

    [Header("Runtime Debug")]
    [SerializeField] [Tooltip("True while the player is considered grounded.")]
    private bool grounded;

    [SerializeField] [Tooltip("How long the current jetpack hold has lasted.")]
    private float currentJetpackHoldTime;

    [SerializeField] [Tooltip("Current resolved jetpack thrust direction in world space.")]
    private Vector2 currentJetpackDirection;

    private float moveInput;
    private float verticalInput;
    private bool jetpackPressedThisFrame;
    private bool jetpackHeld;
    private Vector2 gravityDir;
    private Vector2 tangentDir;

    private void Awake()
    {
        rb = rb != null ? rb : GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        if (startWithFullFuel)
        {
            currentFuel = maxFuel;
        }
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        jetpackPressedThisFrame = Input.GetKeyDown(jetpackKey);
        jetpackHeld = Input.GetKey(jetpackKey);
    }

    private void FixedUpdate()
    {
        if (gravityManager == null)
        {
            return;
        }

        Vector2 gravityVector = gravityManager.GetGravityVector(rb.position);

        if (gravityVector.sqrMagnitude > 0.0001f)
        {
            gravityDir = gravityVector.normalized;
        }
        else
        {
            gravityDir = gravityManager.GetGravityDirection(rb.position);
            gravityVector = gravityDir * gravityStrength;
        }

        tangentDir = new Vector2(-gravityDir.y, gravityDir.x);
        grounded = Physics2D.Raycast(rb.position, gravityDir, groundCheckDistance, groundLayer);

        rb.AddForce(gravityVector * rb.mass, ForceMode2D.Force);

        if (grounded)
        {
            rb.AddForce(gravityDir * stickForce * rb.mass, ForceMode2D.Force);
        }

        ApplySurfaceMovement();
        ApplyJetpack();
        ApplyRotation();
    }

    private void ApplySurfaceMovement()
    {
        Vector2 velocity = rb.linearVelocity;
        float gravityVelocity = Vector2.Dot(velocity, gravityDir);
        float tangentVelocity = Vector2.Dot(velocity, tangentDir);

        float targetTangentVelocity = moveInput * moveSpeed;
        float newTangentVelocity = Mathf.MoveTowards(tangentVelocity, targetTangentVelocity, moveAcceleration * Time.fixedDeltaTime);

        rb.linearVelocity = gravityDir * gravityVelocity + tangentDir * newTangentVelocity;
    }

    private void ApplyJetpack()
    {
        bool canBurst = currentFuel >= fuelCostPerBurst;
        currentJetpackDirection = ResolveJetpackDirection();

        if (jetpackPressedThisFrame && canBurst)
        {
            currentFuel = Mathf.Max(0f, currentFuel - fuelCostPerBurst);
            currentJetpackHoldTime = 0f;
            rb.AddForce(currentJetpackDirection * jetpackBurstImpulse * rb.mass, ForceMode2D.Impulse);
        }

        bool canSustain = jetpackHeld && currentFuel > 0f && currentJetpackHoldTime < maxJetpackHoldDuration;
        if (!canSustain)
        {
            if (!jetpackHeld)
            {
                currentJetpackHoldTime = 0f;
            }
            return;
        }

        float fuelNeededThisFrame = fuelCostPerSecondWhileHolding * Time.fixedDeltaTime;
        if (currentFuel <= 0f)
        {
            return;
        }

        currentFuel = Mathf.Max(0f, currentFuel - fuelNeededThisFrame);
        currentJetpackHoldTime += Time.fixedDeltaTime;
        rb.AddForce(currentJetpackDirection * jetpackSustainForce * rb.mass, ForceMode2D.Force);
    }

    private Vector2 ResolveJetpackDirection()
    {
        Vector2 baseDirection = -gravityDir;
        Vector2 localUp = -gravityDir;
        Vector2 localRight = tangentDir;

        float steeringInfluence = useStrongSteeringOverride ? strongSteeringInfluence : jetpackSteeringInfluence;

        Vector2 inputDirection = localRight * moveInput;
        if (useVerticalInputForJetpackSteering)
        {
            inputDirection += localUp * verticalInput;
        }

        if (inputDirection.sqrMagnitude <= 0.0001f)
        {
            return baseDirection.normalized;
        }

        inputDirection.Normalize();
        Vector2 blended = Vector2.Lerp(baseDirection.normalized, inputDirection, steeringInfluence);
        return blended.sqrMagnitude > 0.0001f ? blended.normalized : baseDirection.normalized;
    }

    private void ApplyRotation()
    {
        float angle = Mathf.Atan2(-gravityDir.y, -gravityDir.x) * Mathf.Rad2Deg + spriteUpOffset;
        rb.MoveRotation(angle);
    }

    public void RestoreFullFuel()
    {
        currentFuel = maxFuel;
        currentJetpackHoldTime = 0f;
    }
}
