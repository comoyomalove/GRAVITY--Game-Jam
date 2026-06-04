using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class NoraPlayerWallMovement2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NoraGravityFieldManager gravityManager;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private Transform groundProbe;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float moveAcceleration = 35f;
    [SerializeField] private float jumpImpulse = 8f;

    [Header("Gravity / Stickiness")]
    [SerializeField] private float gravityStrength = 25f;
    [SerializeField] private float stickForce = 20f;
    [SerializeField] private float groundProbeRadius = 0.12f;

    [Header("Jump")]
    [SerializeField] private int maxJumps = 2;

    [Header("Rotation")]
    [SerializeField] private float spriteUpOffset = 0f;

    private Rigidbody2D rb;
    private float moveInput;
    private bool jumpQueued;

    private int jumpsRemaining;
    private Vector2 gravityDir;
    private Vector2 tangentDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        jumpsRemaining = maxJumps;
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space)||
            Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.UpArrow)
        )
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

        gravityDir = gravityManager.GetGravityDirection(rb.position);
        tangentDir = new Vector2(-gravityDir.y, gravityDir.x);

        bool grounded = IsGrounded();
        Debug.Log("Ground:"+ grounded);
        if (grounded)
        {
            jumpsRemaining = maxJumps;
            Debug.Log("I can jump again");
        }

        rb.AddForce(gravityDir * gravityStrength * rb.mass, ForceMode2D.Force);

        if (grounded)
        {
            rb.AddForce(gravityDir * stickForce * rb.mass, ForceMode2D.Force);
        }

        Vector2 velocity = rb.linearVelocity;
        float gravityVelocity = Vector2.Dot(velocity, gravityDir);
        float tangentVelocity = Vector2.Dot(velocity, tangentDir);

        float targetTangentVelocity = moveInput * moveSpeed;

        float newTangentVelocity = Mathf.MoveTowards(
            tangentVelocity,
            targetTangentVelocity,
            moveAcceleration * Time.fixedDeltaTime
        );

        rb.linearVelocity = tangentDir * newTangentVelocity + gravityDir * gravityVelocity;

        if (jumpQueued && jumpsRemaining > 0)
        {
            rb.AddForce(-gravityDir * jumpImpulse * rb.mass, ForceMode2D.Impulse);
            jumpsRemaining--;
        }

        jumpQueued = false;

        float angle = gravityManager.GetRotationAngle(gravityDir, spriteUpOffset);
        rb.MoveRotation(angle);
    }

    private bool IsGrounded()
    {
        if (groundProbe == null)
        {
            return false;
        }

        return Physics2D.OverlapCircle(groundProbe.position, groundProbeRadius, GroundLayer) != null;
    }
}