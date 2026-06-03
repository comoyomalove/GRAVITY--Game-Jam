using UnityEngine;

public class NoraGravityFieldManager : MonoBehaviour
{
    [Header("Arena")]
    [SerializeField] private Vector2 center = Vector2.zero;
    [SerializeField] private float arenaHalfSize = 10f;

    [Header("Gravity")]
    [SerializeField] private float gravityAcceleration = 25f;

    public float GravityAcceleration => gravityAcceleration;

    public Vector2 GetGravityDirection(Vector2 worldPosition)
    {
        Vector2 local = worldPosition - center;

        if (Mathf.Abs(local.y) >= Mathf.Abs(local.x))
        {
            return local.y >= 0f ? Vector2.up : Vector2.down;
        }

        return local.x >= 0f ? Vector2.right : Vector2.left;
    }

    public Vector2 GetGravityVector(Vector2 worldPosition)
    {
        return GetGravityDirection(worldPosition) * gravityAcceleration;
    }

    public float GetRotationAngle(Vector2 gravityDirection, float spriteUpOffset = 0f)
    {
        Vector2 desiredUp = -gravityDirection;
        float angle = Vector2.SignedAngle(Vector2.up, desiredUp);
        return angle + spriteUpOffset;
    }
}