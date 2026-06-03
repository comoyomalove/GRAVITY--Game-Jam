using UnityEngine;
using System.Collections;

public class NoraBreakablePlatform : MonoBehaviour
{
    public float breakDelay = 2f;
    public float respawnDelay = 5f;

    private SpriteRenderer[] spriteRenderers;
    private Collider2D platformCollider;

    private bool breaking = false;

    private void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
{
    Debug.Log("Something touched me!");

    if (breaking)
        return;

    if (collision.gameObject.CompareTag("Player"))
    {
        Debug.Log("Player touched me!");
        StartCoroutine(BreakRoutine());
    }
}

    IEnumerator BreakRoutine()
    {
        breaking = true;

        yield return new WaitForSeconds(breakDelay);

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = false;
        }        
        platformCollider.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = true;
        }
        platformCollider.enabled = true;

        breaking = false;
    }
}