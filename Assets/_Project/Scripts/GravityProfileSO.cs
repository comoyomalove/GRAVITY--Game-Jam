using UnityEngine;

[CreateAssetMenu(fileName = "GravityProfile", menuName = "Gravity/Gravity Profile")]
public class GravityProfileSO : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] [Tooltip("Friendly display name for this gravity profile.")]
    private string profileName = "Default Profile";

    [Header("Force")]
    [SerializeField] [Tooltip("Base pull strength for this gravity zone.")]
    [Min(0f)]
    private float gravityStrength = 10f;

    [SerializeField] [Tooltip("How far this profile's gravity can influence objects.")]
    [Min(0.1f)]
    private float influenceRadius = 5f;

    [SerializeField] [Tooltip("How quickly pull strength fades with distance. Higher = stronger near center, weaker at edge.")]
    [Min(0.1f)]
    private float falloffPower = 2f;

    [Header("Priority")]
    [SerializeField] [Tooltip("Used when multiple zones overlap. Higher priority wins if strengths are equal.")]
    private int priority = 0;

    [SerializeField] [Tooltip("Enable/disable this profile without deleting it.")]
    private bool isEnabled = true;

    public string ProfileName => profileName;
    public float GravityStrength => gravityStrength;
    public float InfluenceRadius => influenceRadius;
    public float FalloffPower => falloffPower;
    public int Priority => priority;
    public bool IsEnabled => isEnabled;
}
