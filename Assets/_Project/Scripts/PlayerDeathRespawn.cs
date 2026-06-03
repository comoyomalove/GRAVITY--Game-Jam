using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerDeathRespawn : MonoBehaviour
{
    [Header("References")]
    [SerializeField] [Tooltip("Respawn point where the player returns after death.")]
    private RespawnPoint respawnPoint;

    [SerializeField] [Tooltip("Optional Nora movement script to disable briefly during respawn.")]
    private NoraPlayerWallMovement2D wallMovement;

    [SerializeField] [Tooltip("Optional Nora gravity script to disable briefly during respawn.")]
    private NoraPlayerGravity playerGravity;

    [Header("Respawn Settings")]
    [SerializeField] [Tooltip("Delay in seconds before the player reappears at the respawn point.")]
    [Min(0f)]
    private float respawnDelay = 0.15f;

    [SerializeField] [Tooltip("If enabled, the player's velocity is fully reset when respawning.")]
    private bool resetVelocityOnRespawn = true;

    [SerializeField] [Tooltip("If enabled, the player respawns with full fuel.")]
    private bool restoreFullFuelOnRespawn = true;

    [SerializeField] [Tooltip("Fuel amount to restore if full fuel restore is handled through a specific component later.")]
    [Min(0f)]
    private float fallbackFuelAmount = 100f;

    private Rigidbody2D rb;
    private bool isRespawning;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (wallMovement == null)
        {
            wallMovement = GetComponent<NoraPlayerWallMovement2D>();
        }

        if (playerGravity == null)
        {
            playerGravity = GetComponent<NoraPlayerGravity>();
        }
    }

    public void KillAndRespawn()
    {
        if (isRespawning)
        {
            return;
        }

        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        isRespawning = true;
        SetGameplayScriptsEnabled(false);

        if (resetVelocityOnRespawn && rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        yield return new WaitForSeconds(respawnDelay);

        if (respawnPoint != null && respawnPoint.IsActive)
        {
            transform.position = respawnPoint.transform.position;
        }

        if (resetVelocityOnRespawn && rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        RestoreFuelIfPossible();
        SetGameplayScriptsEnabled(true);
        isRespawning = false;
    }

    private void SetGameplayScriptsEnabled(bool enabledState)
    {
        if (wallMovement != null)
        {
            wallMovement.enabled = enabledState;
        }

        if (playerGravity != null)
        {
            playerGravity.enabled = enabledState;
        }
    }

    private void RestoreFuelIfPossible()
    {
        if (!restoreFullFuelOnRespawn)
        {
            return;
        }

        var fuelReceiver = GetComponent<IPlayerFuelReceiver>();
        if (fuelReceiver != null)
        {
            fuelReceiver.RestoreFullFuel();
            return;
        }

        var fallbackFuel = GetComponent<LegacyFuelStore>();
        if (fallbackFuel != null)
        {
            fallbackFuel.SetFuelToMax();
            return;
        }
    }
}
