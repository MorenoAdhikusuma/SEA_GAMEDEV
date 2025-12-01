using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator anim;
    public Rigidbody2D rb;
    public Transform firePoint;
    public Transform firePoint2;

    public GameObject bulletPrefab;

    private SpriteRenderer sr;

    [Header("Base Stats")]
    public float maxHealth = 100f;
    public float currentHealth;

    public float moveSpeed = 2f;
    public float stopDistance = 5f;

    [Header("Shooting")]
    public float shootInterval = 2f;
    private float shootTimer = 0f;
    public float bulletSpeed = 7f;

    // Shooting Patterns
    public enum ShootPattern { Single, Burst, Spread }
    public ShootPattern pattern = ShootPattern.Single;

    [Header("Burst Settings")]
    public int burstCount = 3;
    public float burstDelay = 0.15f;

    [Header("Spread Settings")]
    public int spreadCount = 5;
    public float spreadAngle = 30f;

    private bool isDead = false;
    private bool playerInRange = false;

    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();

        UpdatePhase(); // initialize phase on start
    }

    void Update()
    {
        if (isDead || player == null) return;

        shootTimer -= Time.deltaTime;
        FlipTowardsPlayer();

        float dist = Vector2.Distance(transform.position, player.position);

        // Monitor phase changes
        UpdatePhase();

        if (playerInRange)
        {
            HandleMovement(dist);
            TryShoot(dist);
        }
        else
        {
            anim.SetBool("walk", false);
        }
    }

    // ---------------------------------------------------------
    //                PHASE SYSTEM
    // ---------------------------------------------------------
    void UpdatePhase()
    {
        float hpPercent = (currentHealth / maxHealth) * 100f;

        if (hpPercent > 60f)
        {
            // PHASE 1: Single shot (basic)
            pattern = ShootPattern.Single;
            shootInterval = 1.5f;
            moveSpeed = 2f;
        }
        else if (hpPercent > 30f)
        {
            // PHASE 2: Burst
            pattern = ShootPattern.Burst;
            shootInterval = 1.3f;
            moveSpeed = 2.5f;
            burstCount = 3;
        }
        else
        {
            // PHASE 3: Final Spread (danger)
            pattern = ShootPattern.Spread;
            shootInterval = 1f;
            moveSpeed = 3f;
            spreadCount = 5;
            spreadAngle = 45f;
        }
    }

    // ---------------------------------------------------------
    void FlipTowardsPlayer()
    {
        sr.flipX = player.position.x > transform.position.x;
    }

    // ---------------------------------------------------------
    void HandleMovement(float dist)
    {
        if (dist > stopDistance)
        {
            anim.SetBool("walk", true);
            Vector2 target = new Vector2(player.position.x, transform.position.y);
            rb.MovePosition(Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime));
        }
        else
        {
            anim.SetBool("walk", false);
        }
    }

    // ---------------------------------------------------------
    void TryShoot(float dist)
    {
        if (dist <= stopDistance && shootTimer <= 0f)
        {
            anim.SetTrigger("shoot");
            Shoot();
            shootTimer = shootInterval;
        }
    }

    // ---------------------------------------------------------
    //                  SHOOTING PATTERNS
    // ---------------------------------------------------------
    void Shoot()
    {
        switch (pattern)
        {
            case ShootPattern.Single:
                FireSingle();
                break;

            case ShootPattern.Burst:
                StartCoroutine(FireBurst());
                break;

            case ShootPattern.Spread:
                FireSpread();
                break;
        }
    }

    void FireSingle()
    {
        ShootBullet(player.position - firePoint.position);
    }

    System.Collections.IEnumerator FireBurst()
    {
        for (int i = 0; i < burstCount; i++)
        {
            ShootBullet(player.position - firePoint.position);
            yield return new WaitForSeconds(burstDelay);
        }
    }

    void FireSpread()
    {
        Vector2 dir = (player.position - firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float startAngle = baseAngle - (spreadAngle / 2f);
        float step = spreadAngle / (spreadCount - 1);

        for (int i = 0; i < spreadCount; i++)
        {
            float angle = startAngle + step * i;
            Vector2 shotDir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            ShootBullet(shotDir);
        }
    }

    // ---------------------------------------------------------
    void ShootBullet(Vector2 direction)
    {
        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        b.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * bulletSpeed;
    }

    // ---------------------------------------------------------
    //                      DAMAGE
    // ---------------------------------------------------------
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        anim.SetTrigger("hit");

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;

        anim.SetTrigger("death");
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;

        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;

        Destroy(gameObject, 2f);
    }

    // ---------------------------------------------------------
    //                PLAYER DETECTION
    // ---------------------------------------------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            anim.SetBool("walk", false);
        }
    }
}
