using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    // ======== MOVEMENT / STATE ========
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    public float attackRange = 1.5f;
    public int maxHealth = 100;

    private int currentHealth;
    private bool isDead = false;
    private bool isAttacking = false;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private Transform player;
    private Animator animator;

    // ======== RANGED ATTACK ========
    public GameObject projectilePrefab;
    public float projectileSpeed = 8f;
    public float fireRate = 2f;
    public float spreadAngle = 20f;
    public Transform firePoint;

    private float fireTimer;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        currentHealth = maxHealth;
        fireTimer = fireRate;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // ---- FLIP TOWARD PLAYER ----
        if (player.position.x > transform.position.x)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        // ---- PLAYER TOO FAR → idle ----
        if (distance > detectionRange)
        {
            animator.SetInteger("Boss_stat", 0);
            return;
        }

        // ---- ATTACK RANGE → melee ----
        if (distance <= attackRange)
        {
            if (!isAttacking)
                StartCoroutine(MeleeRoutine());

            return;
        }

        // ---- RANGED SHOOTING (only if not melee attacking) ----
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0 && !isAttacking)
        {
            ShootAtPlayerWithSpread();
            fireTimer = fireRate;
        }

        // ---- WALK TOWARD PLAYER ----
        MoveTowardsPlayer();
    }

    // =======================================
    // WALKING
    // =======================================
    void MoveTowardsPlayer()
    {
        animator.SetInteger("Boss_stat", 1); // walk

        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += new Vector3(dir.x, 0, 0) * moveSpeed * Time.deltaTime;
    }

    // =======================================
    // MELEE ATTACK
    // =======================================
    IEnumerator MeleeRoutine()
    {
        isAttacking = true;
        animator.SetTrigger("Melee");
        yield return new WaitForSeconds(0.7f); // match your animation
        isAttacking = false;
    }

    // =======================================
    // RANGED ATTACK
    // =======================================
    void ShootAtPlayerWithSpread()
    {
        if (player == null) return;

        Vector2 baseDir = (player.position - firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        float randomOffset = Random.Range(-spreadAngle, spreadAngle);
        float finalAngle = baseAngle + randomOffset;

        float spriteOffset = -90f;
        float finalAngleWithOffset = finalAngle + spriteOffset;

        float rad = finalAngle * Mathf.Deg2Rad;
        Vector2 finalDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

        GameObject proj = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.Euler(0f, 0f, finalAngleWithOffset)
        );

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        rb.linearVelocity = finalDir * projectileSpeed;
    }

    // =======================================
    // DAMAGE / HIT / DEATH
    // =======================================
    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;

        if (currentHealth > 0)
        {
            animator.SetTrigger("hit");
        }
        else
        {
            StartCoroutine(DieRoutine());
        }
    }

    IEnumerator DieRoutine()
    {
        isDead = true;
        animator.SetTrigger("death");
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }
}
