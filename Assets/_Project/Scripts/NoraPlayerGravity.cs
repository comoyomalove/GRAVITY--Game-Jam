using UnityEngine;

public class NoraPlayerGravity : MonoBehaviour
{
    [Header("References")]

    public NoraGravityFieldManager gravityManager;

    public float gravityStrength = 20f;

    [SerializeField] [Tooltip("Player Rigidbody2D that receives gravity force.")]
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (gravityManager == null)
            return;

        Vector2 gravityDirection =
            gravityManager.GetGravityDirection(transform.position);

        rb.AddForce(
            gravityDirection * gravityStrength,
            ForceMode2D.Force
        );

        RotatePlayer(gravityDirection);
    }

    private void RotatePlayer(Vector2 gravityDirection)
    {
        Vector2 feetDirection = -gravityDirection;

        float angle =
            Vector2.SignedAngle(Vector2.up, feetDirection);

        transform.rotation =
            Quaternion.Euler(0, 0, angle);
    }
}