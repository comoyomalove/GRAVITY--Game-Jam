
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class NoraPlayerWallMovement2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NoraGravityFieldManager gravityManager;
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float moveAcceleration = 35f;
    [SerializeField] private float jumpImpulse = 8f;

    [Header("Gravity / Stickiness")]
    [SerializeField] private float gravityStrength = 25f;
    [SerializeField] private float stickForce = 20f;
    [SerializeField] private float groundCheckDistance = 0.6f;

    [Header("Rotation")]
    [SerializeField] private float spriteUpOffset = 0f;

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

        gravityDir = gravityManager.GetGravityDirection(rb.position);
        tangentDir = new Vector2(-gravityDir.y, gravityDir.x);

        grounded = Physics2D.Raycast(rb.position, gravityDir, groundCheckDistance, groundLayer);

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

        if (jumpQueued && grounded)
        {
            rb.AddForce(-gravityDir * jumpImpulse * rb.mass, ForceMode2D.Impulse);
            grounded = false;
        }

        jumpQueued = false;

        float angle = gravityManager.GetRotationAngle(gravityDir, spriteUpOffset);
        rb.MoveRotation(angle);
    }

    private void OnDrawGizmosSelected()
    {
        if (rb == null || gravityManager == null)
        {
            return;
        }

        Vector2 dir = gravityManager.GetGravityDirection(transform.position);
        Gizmos.color = grounded ? Color.green : Color.red;
        Gizmos.DrawLine(rb.position, rb.position + dir * groundCheckDistance);
    }
}