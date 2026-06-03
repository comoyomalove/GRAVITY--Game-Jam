using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class NoraPlayerWallMovement2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] [Tooltip("Gravity manager that defines current gravity direction in the arena.")]
    private NoraGravityFieldManager gravityManager;

    [SerializeField] [Tooltip("Layer mask used to detect ground surfaces relative to active gravity.")]
    private LayerMask groundLayer;

    [Header("Movement")]
    [SerializeField] [Tooltip("Base movement speed along the gravity-aligned tangent direction.")]
    [Min(0f)]
    private float moveSpeed = 8f;

    [SerializeField] [Tooltip("How quickly the player reaches target tangent speed.")]
    [Min(0f)]
    private float moveAcceleration = 35f;

    [SerializeField] [Tooltip("Impulse strength applied when jumping away from gravity direction.")]
    [Min(0f)]
    private float jumpImpulse = 8f;

    [Header("Gravity / Stickiness")]
    [SerializeField] [Tooltip("Fallback custom gravity force strength applied if zone-driven vector is unavailable.")]
    [Min(0f)]
    private float gravityStrength = 25f;

    [SerializeField] [Tooltip("Extra stick force to keep player attached to surface when grounded.")]
    [Min(0f)]
    private float stickForce = 20f;

    [SerializeField] [Tooltip("Distance of raycast used to detect if player is grounded.")]
    [Min(0.01f)]
    private float groundCheckDistance = 0.6f;

    [Header("Rotation")]
    [SerializeField] [Tooltip("Sprite rotation offset in degrees to align artwork to gravity direction.")]
    [Range(-360f, 360f)]
    private float spriteUpOffset = 0f;

    [SerializeField] [Tooltip("Player Rigidbody2D used for movement and force application.")]
    private Rigidbody2D rb;

    private float moveInput;
    private bool jumpQueued;
    private bool grounded;

    private Vector2 gravityDir;
    private Vector2 tangentDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpQueued = true;
        }
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

        Vector2 velocity = rb.linearVelocity;
        float gravityVelocity = Vector2.Dot(velocity, gravityDir);
        float tangentVelocity = Vector2.Dot(velocity, tangentDir);

        float targetTangentVelocity = moveInput * moveSpeed;
        float newTangentVelocity = Mathf.MoveTowards(tangentVelocity, targetTangentVelocity, moveAcceleration * Time.fixedDeltaTime);

        rb.linearVelocity = gravityDir * gravityVelocity + tangentDir * newTangentVelocity;

        if (jumpQueued && grounded)
        {
            rb.AddForce(-gravityDir * jumpImpulse * rb.mass, ForceMode2D.Impulse);
        }

        jumpQueued = false;

        float angle = Mathf.Atan2(-gravityDir.y, -gravityDir.x) * Mathf.Rad2Deg + spriteUpOffset;
        rb.MoveRotation(angle);
    }
}
