using UnityEngine;

public class NoraPlayerGravity : MonoBehaviour
{
    [Header("References")]
    [SerializeField] [Tooltip("Gravity manager that provides gravity direction and zone-driven strength.")]
    private NoraGravityFieldManager gravityManager;

    [Header("Fallback Gravity")]
    [SerializeField] [Tooltip("Fallback gravity strength used only if no gravity vector is returned by the manager.")]
    [Min(0f)]
    private float gravityStrength = 20f;

    [SerializeField] [Tooltip("Player Rigidbody2D that receives gravity force.")]
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (gravityManager == null || rb == null)
        {
            return;
        }

        Vector2 gravityVector = gravityManager.GetGravityVector(transform.position);
        Vector2 gravityDirection;

        if (gravityVector.sqrMagnitude > 0.0001f)
        {
            gravityDirection = gravityVector.normalized;
            rb.AddForce(gravityVector, ForceMode2D.Force);
        }
        else
        {
            gravityDirection = gravityManager.GetGravityDirection(transform.position);
            rb.AddForce(gravityDirection * gravityStrength, ForceMode2D.Force);
        }

        RotatePlayer(gravityDirection);
    }

    private void RotatePlayer(Vector2 gravityDirection)
    {
        Vector2 feetDirection = -gravityDirection;
        float angle = Vector2.SignedAngle(Vector2.up, feetDirection);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
