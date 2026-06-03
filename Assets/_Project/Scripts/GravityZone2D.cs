using UnityEngine;

public class GravityZone2D : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] [Tooltip("Shared gravity profile for this zone.")]
    private GravityProfileSO profile;

    [Header("Local Override")]
    [SerializeField] [Tooltip("If enabled, this zone uses local values instead of the profile values.")]
    private bool useLocalOverride = false;

    [SerializeField] [Tooltip("Local pull strength override.")]
    [Min(0f)]
    private float localGravityStrength = 10f;

    [SerializeField] [Tooltip("Local influence radius override.")]
    [Min(0.1f)]
    private float localInfluenceRadius = 5f;

    [SerializeField] [Tooltip("Local falloff power override.")]
    [Min(0.1f)]
    private float localFalloffPower = 2f;

    [SerializeField] [Tooltip("Local priority override.")]
    private int localPriority = 0;

    [SerializeField] [Tooltip("Enable/disable this zone.")]
    private bool zoneEnabled = true;

    [Header("Debug")]
    [SerializeField] [Tooltip("Draw radius gizmo in scene view.")]
    private bool drawGizmo = true;

    [SerializeField] [Tooltip("Scene gizmo color for this zone.")]
    private Color gizmoColor = new Color(0.2f, 0.8f, 1f, 0.4f);

    public bool IsZoneEnabled => zoneEnabled;

    public int GetPriority()
    {
        if (useLocalOverride)
        {
            return localPriority;
        }

        if (profile == null)
        {
            return 0;
        }

        return profile.Priority;
    }

    public bool TryEvaluate(Vector2 targetPosition, out Vector2 gravityDirection, out float effectiveStrength)
    {
        gravityDirection = Vector2.zero;
        effectiveStrength = 0f;

        if (!zoneEnabled)
        {
            return false;
        }

        if (!useLocalOverride && (profile == null || !profile.IsEnabled))
        {
            return false;
        }

        float strength = useLocalOverride ? localGravityStrength : profile.GravityStrength;
        float radius = useLocalOverride ? localInfluenceRadius : profile.InfluenceRadius;
        float falloffPower = useLocalOverride ? localFalloffPower : profile.FalloffPower;

        if (radius <= 0f || strength <= 0f)
        {
            return false;
        }

        Vector2 toZone = (Vector2)transform.position - targetPosition;
        float distance = toZone.magnitude;

        if (distance > radius)
        {
            return false;
        }

        gravityDirection = distance > 0.0001f ? toZone / distance : Vector2.down;

        float normalizedDistance = Mathf.Clamp01(distance / radius);
        float falloff = Mathf.Pow(1f - normalizedDistance, Mathf.Max(0.1f, falloffPower));
        effectiveStrength = strength * falloff;

        return effectiveStrength > 0f;
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmo)
        {
            return;
        }

        float radius = useLocalOverride
            ? Mathf.Max(0f, localInfluenceRadius)
            : (profile != null ? Mathf.Max(0f, profile.InfluenceRadius) : 0f);

        if (radius <= 0f)
        {
            return;
        }

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
