using UnityEngine;

public class NoraSpike : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private NoraGameManager gameManager;
    [SerializeField] private float timePenalty = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        NoraPlayerWallMovement2D player =
            other.GetComponentInParent<NoraPlayerWallMovement2D>();

        if (player == null)
            return;

        player.transform.position = respawnPoint.position;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (gameManager != null)
        {
            gameManager.LoseTime(timePenalty);
        }
    }
}