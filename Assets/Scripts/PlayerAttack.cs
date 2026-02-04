using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRadius = 0.5f;
    public LayerMask enemyLayer;
    public float attackCooldown = 0.3f;
    public float knockbackForce = 6f;
    public int attackDamage = 1;

    float nextAttackTime = 0f;

    void Update()
    {
        bool attackPressed = Input.GetKeyDown(KeyCode.G) ||
                             Input.GetButtonDown("Fire1");

        if (attackPressed && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            enemyLayer
        );

        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponentInParent<Enemy>();
            EnemyHealth health = hit.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                // Knockback
                Vector2 knockDir = (enemy.transform.position - transform.position).normalized;
                enemy.ApplyKnockback(knockDir * knockbackForce);
            }

            if (health != null)
            {
                // Always 1 damage
                health.TakeDamage(1);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
