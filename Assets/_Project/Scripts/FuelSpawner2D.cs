using UnityEngine;

public class FuelSpawner2D : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] [Tooltip("Fuel pickup prefab that will be spawned inside the configured rectangle area.")]
    private GameObject fuelPickupPrefab;

    [SerializeField] [Tooltip("How many seconds to wait between spawn attempts.")]
    [Min(0.1f)]
    private float spawnInterval = 5f;

    [SerializeField] [Tooltip("Maximum number of active spawned fuel pickups allowed at the same time.")]
    [Min(1)]
    private int maxActiveFuelPickups = 3;

    [SerializeField] [Tooltip("If enabled, the spawner starts spawning automatically on play.")]
    private bool autoStartSpawning = true;

    [Header("Spawn Area")]
    [SerializeField] [Tooltip("Center of the rectangular spawn area in local space.")]
    private Vector2 spawnAreaCenter = Vector2.zero;

    [SerializeField] [Tooltip("Size of the rectangular spawn area in world units.")]
    private Vector2 spawnAreaSize = new Vector2(10f, 6f);

    [Header("Debug")]
    [SerializeField] [Tooltip("Draw the spawn area in the scene view for easier tuning.")]
    private bool drawSpawnAreaGizmo = true;

    [SerializeField] [Tooltip("Gizmo color used for the spawn area rectangle.")]
    private Color gizmoColor = new Color(1f, 0.8f, 0.2f, 0.35f);

    private float spawnTimer;
    private int activePickupCount;

    private void Start()
    {
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        if (!autoStartSpawning || fuelPickupPrefab == null)
        {
            return;
        }

        if (activePickupCount >= maxActiveFuelPickups)
        {
            return;
        }

        spawnTimer -= Time.deltaTime;
        if (spawnTimer > 0f)
        {
            return;
        }

        SpawnFuelPickup();
        spawnTimer = spawnInterval;
    }

    private void SpawnFuelPickup()
    {
        Vector2 worldCenter = (Vector2)transform.position + spawnAreaCenter;
        Vector2 halfSize = spawnAreaSize * 0.5f;

        Vector2 spawnPosition = new Vector2(
            Random.Range(worldCenter.x - halfSize.x, worldCenter.x + halfSize.x),
            Random.Range(worldCenter.y - halfSize.y, worldCenter.y + halfSize.y)
        );

        GameObject pickup = Instantiate(fuelPickupPrefab, spawnPosition, Quaternion.identity);
        activePickupCount++;
        pickup.AddComponent<FuelPickupSpawnTracker>().Initialize(this);
    }

    public void NotifyPickupDestroyed()
    {
        activePickupCount = Mathf.Max(0, activePickupCount - 1);
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawSpawnAreaGizmo)
        {
            return;
        }

        Gizmos.color = gizmoColor;
        Vector3 worldCenter = transform.position + (Vector3)spawnAreaCenter;
        Gizmos.DrawWireCube(worldCenter, spawnAreaSize);
    }
}
