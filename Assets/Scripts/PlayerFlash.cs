using UnityEngine;
using System.Collections;

public class PlayerFlash : MonoBehaviour
{
    [Header("Player Health")]
    public int playerHealth = 10;   // starting health
    public int maxHealth = 10;

    public SpriteRenderer spriteRenderer;

    public Color flashColor = Color.white;
    public float flashDuration = 0.15f;
    public float invincibleTime = 3f;

    private Color originalColor;
    public bool isInvincible = false;

    private int enemyLayer;
    private int playerLayer;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        enemyLayer = LayerMask.NameToLayer("Enemy");
        playerLayer = LayerMask.NameToLayer("Player");

        // Initialize UI bar
        UIManager.Instance.SetHealth(playerHealth);
    }

    public void FlashNow()
    {
        if (!isInvincible)
            StartCoroutine(InvincibleCoroutine());
    }

    private IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;

        // Disable collision between player ↔ enemy
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        float endTime = Time.time + invincibleTime;

        // Flash during invincible state
        while (Time.time < endTime)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }

        // Restore normal state
        spriteRenderer.color = originalColor;
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);

        isInvincible = false;
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        playerHealth -= amount;
        UIManager.Instance.SetHealth(playerHealth);

        FlashNow();

        if (playerHealth <= 0)
        {
            Debug.Log("Player Died!");
            // TODO: Respawn or Game Over Screen
        }
    }
}
