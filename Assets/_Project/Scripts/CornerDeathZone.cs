using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CornerDeathZone : MonoBehaviour
{
    [Header("Death Zone")]
    [SerializeField] [Tooltip("If enabled, the zone will kill the player on contact.")]
    private bool zoneActive = true;

    [SerializeField] [Tooltip("Tag used to identify the player object.")]
    private string playerTag = "Player";

    private void Reset()
    {
        Collider2D collider2D = GetComponent<Collider2D>();
        if (collider2D != null)
        {
            collider2D.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!zoneActive)
        {
            return;
        }

        if (!other.CompareTag(playerTag))
        {
            return;
        }

        PlayerDeathRespawn deathRespawn = other.GetComponent<PlayerDeathRespawn>();
        if (deathRespawn != null)
        {
            deathRespawn.KillAndRespawn();
        }
    }
}
