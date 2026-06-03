using UnityEngine;

public class NoraGravityFieldManager : MonoBehaviour
{
    [Header("Arena")]
    [SerializeField] [Tooltip("Center of the square arena in world space.")]
    private Vector2 center = Vector2.zero;

    [SerializeField] [Tooltip("Half-size of the arena used by legacy gravity mode.")]
    [Min(0.1f)]
    private float arenaHalfSize = 10f;

    [Header("Legacy Gravity")]
    [SerializeField] [Tooltip("Fallback acceleration for legacy cardinal gravity mode.")]
    [Min(0f)]
    private float gravityAcceleration = 25f;

    [Header("Zone Gravity")]
    [SerializeField] [Tooltip("If enabled, gravity is resolved from GravityZone2D components in the scene.")]
    private bool useGravityZones = true;

    [SerializeField] [Tooltip("If enabled, manager auto-discovers GravityZone2D components on Start.")]
    private bool autoFindZones = true;

    [SerializeField] [Tooltip("Optional manual zone list. Used when auto-find is disabled or for explicit control.")]
    private GravityZone2D[] gravityZones;

    [SerializeField] [Tooltip("When true, strongest effective pull wins. If equal, higher priority wins.")]
    private bool strongestWins = true;

    public float GravityAcceleration => gravityAcceleration;

    private void Start()
    {
        if (useGravityZones && autoFindZones)
        {
            gravityZones = FindObjectsByType<GravityZone2D>(FindObjectsSortMode.None);
        }
    }

    public Vector2 GetGravityDirection(Vector2 worldPosition)
    {
        if (useGravityZones && TryGetZoneGravity(worldPosition, out Vector2 zoneDir, out float _))
        {
            return zoneDir;
        }

        Vector2 local = worldPosition - center;

        if (Mathf.Abs(local.y) >= Mathf.Abs(local.x))
        {
            return local.y >= 0f ? Vector2.up : Vector2.down;
        }

        return local.x >= 0f ? Vector2.right : Vector2.left;
    }

    public Vector2 GetGravityVector(Vector2 worldPosition)
    {
        if (useGravityZones && TryGetZoneGravity(worldPosition, out Vector2 zoneDir, out float strength))
        {
            return zoneDir * strength;
        }

        return GetGravityDirection(worldPosition) * gravityAcceleration;
    }

    public float GetRotationAngle(Vector2 gravityDirection, float spriteUpOffset = 0f)
    {
        Vector2 desiredUp = -gravityDirection;
        float angle = Vector2.SignedAngle(Vector2.up, desiredUp);
        return angle + spriteUpOffset;
    }

    private bool TryGetZoneGravity(Vector2 worldPosition, out Vector2 gravityDirection, out float strength)
    {
        gravityDirection = Vector2.zero;
        strength = 0f;

        if (gravityZones == null || gravityZones.Length == 0)
        {
            return false;
        }

        GravityZone2D bestZone = null;
        float bestStrength = 0f;
        Vector2 bestDirection = Vector2.zero;
        int bestPriority = int.MinValue;

        foreach (GravityZone2D zone in gravityZones)
        {
            if (zone == null || !zone.IsZoneEnabled)
            {
                continue;
            }

            if (!zone.TryEvaluate(worldPosition, out Vector2 dir, out float currentStrength))
            {
                continue;
            }

            int currentPriority = zone.GetPriority();

            if (bestZone == null)
            {
                bestZone = zone;
                bestStrength = currentStrength;
                bestDirection = dir;
                bestPriority = currentPriority;
                continue;
            }

            if (strongestWins)
            {
                if (currentStrength > bestStrength || (Mathf.Approximately(currentStrength, bestStrength) && currentPriority > bestPriority))
                {
                    bestZone = zone;
                    bestStrength = currentStrength;
                    bestDirection = dir;
                    bestPriority = currentPriority;
                }
            }
            else
            {
                if (currentPriority > bestPriority || (currentPriority == bestPriority && currentStrength > bestStrength))
                {
                    bestZone = zone;
                    bestStrength = currentStrength;
                    bestDirection = dir;
                    bestPriority = currentPriority;
                }
            }
        }

        if (bestZone == null)
        {
            return false;
        }

        gravityDirection = bestDirection;
        strength = bestStrength;
        return true;
    }
}
