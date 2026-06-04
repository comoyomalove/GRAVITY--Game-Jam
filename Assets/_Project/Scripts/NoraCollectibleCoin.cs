using System.Collections;
using UnityEngine;

public class NoraCollectibleCoin : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NoraGameManager gameManager;

    [Header("Spawn Rules")]
    [SerializeField] private LayerMask blockedLayers;
    [SerializeField] private Vector2 arenaCenter = Vector2.zero;
    [SerializeField] private Vector2 arenaSize = new Vector2(18f, 18f);
    [SerializeField] private float edgeMargin = 1f;
    [SerializeField] private float minDistanceFromPrevious = 3f;
    [SerializeField] private float coinRadius = 0.35f;
    [SerializeField] private int maxAttempts = 80;

    [Header("Respawn")]
    [SerializeField] private float respawnDelay = 0.3f;

    private SpriteRenderer[] spriteRenderers;
    private Collider2D[] colliders;
    private Vector2 lastSpawnPosition;
    private bool collecting;
    private int points = 0;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        colliders = GetComponentsInChildren<Collider2D>(true);
        lastSpawnPosition = transform.position;
    }

    private void Start()
    {
        MoveToSafeSpot(null);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Coin touched!");
        if (collecting)
        {
            return;
        }

        if (other.GetComponentInParent<NoraPlayerWallMovement2D>() == null)
        {
            return;
        }

        StartCoroutine(CollectRoutine());
    }

    private IEnumerator CollectRoutine()
    {
        collecting = true;

        SetVisible(false);

        if (points >= 0)
        {
            Debug.Log("Adding Score!");
            points += 1;
        }

        yield return new WaitForSeconds(respawnDelay);

        MoveToSafeSpot(lastSpawnPosition);

        SetVisible(true);
        collecting = false;
    }

    private void MoveToSafeSpot(Vector2? previousPosition)
    {
        Vector2 newPosition = FindSafePosition(previousPosition);
        transform.position = newPosition;
        lastSpawnPosition = newPosition;
    }

    private Vector2 FindSafePosition(Vector2? previousPosition)
    {
        float halfWidth = arenaSize.x * 0.5f;
        float halfHeight = arenaSize.y * 0.5f;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector2 candidate = new Vector2(
                Random.Range(arenaCenter.x - halfWidth + edgeMargin, arenaCenter.x + halfWidth - edgeMargin),
                Random.Range(arenaCenter.y - halfHeight + edgeMargin, arenaCenter.y + halfHeight - edgeMargin)
            );

            if (previousPosition.HasValue &&
                Vector2.Distance(candidate, previousPosition.Value) < minDistanceFromPrevious)
            {
                continue;
            }

            if (Physics2D.OverlapCircle(candidate, coinRadius, blockedLayers) != null)
            {
                continue;
            }

            return candidate;
        }

        return arenaCenter;
    }

    private void SetVisible(bool visible)
    {
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = visible;
        }

        foreach (Collider2D col in colliders)
        {
            col.enabled = visible;
        }
    }
}