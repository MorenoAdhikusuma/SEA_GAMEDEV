using UnityEngine;

public class BossAI : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 8f;
    public float fireRate = 1f;
    public float spreadAngle = 20f;
    public Transform firePoint;

    public Transform player;

    private float fireTimer;

    void Update()
    {
        if (player == null) return;

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0)
        {
            ShootAtPlayerWithSpread();
            fireTimer = fireRate;
        }
    }

    void ShootAtPlayerWithSpread()
    {
        // Base direction toward player
        Vector2 baseDir = (player.position - firePoint.position).normalized;

        // Convert direction to angle
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        // Add spread
        float randomOffset = Random.Range(-spreadAngle, spreadAngle);
        float finalAngle = baseAngle + randomOffset;

        // Fix offset because projectile sprite points UP by default
        float spriteOffset = -90f;
        float finalAngleWithOffset = finalAngle + spriteOffset;

        // Convert final angle to direction
        float rad = finalAngle * Mathf.Deg2Rad;
        Vector2 finalDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

        // Spawn projectile
        GameObject proj = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.Euler(0f, 0f, finalAngleWithOffset)
        );

        // Move projectile
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        rb.linearVelocity = finalDir * projectileSpeed;
    }
}
