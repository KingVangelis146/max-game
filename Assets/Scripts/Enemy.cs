using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 1;          // Set: Ant = 1, Lizard = 2
    public int contactDamage = 1;      // Will match maxHealth automatically
    private int currentHealth;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public string playerTag = "Player";

    private Transform player;
    private float attackCooldown = 0.2f;
    private float nextAttackTime = 0f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        // Assign health + contact damage
        currentHealth = maxHealth;
        contactDamage = maxHealth;   // IMPORTANT → damage equals health

        // Find the player
        GameObject p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null)
            player = p.transform;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float dir = Mathf.Sign(player.position.x - rb.position.x);

        rb.linearVelocity = new Vector2(
            dir * moveSpeed,
            rb.linearVelocity.y
        );
    }

    // ----------------------------------------------------------
    // ENEMY TAKES DAMAGE FROM PLAYER ATTACK
    // ----------------------------------------------------------
    public void ApplyKnockback(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        PlayerFlash pf = other.GetComponentInParent<PlayerFlash>();
        if (pf != null)
            TryDamage(pf);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        PlayerFlash pf = other.GetComponentInParent<PlayerFlash>();
        if (pf != null)
            TryDamage(pf);
    }

    void TryDamage(PlayerFlash pf)
    {
        if (pf.isInvincible)
            return;

        if (Time.time >= nextAttackTime)
        {
            pf.TakeDamage(contactDamage);
            nextAttackTime = Time.time + attackCooldown;
        }
    }
}
