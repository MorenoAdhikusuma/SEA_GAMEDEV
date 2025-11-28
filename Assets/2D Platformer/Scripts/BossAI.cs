using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator anim;
    public Rigidbody2D rb;
    public Transform firePoint;

    [Header("Stats")]
    public float maxHealth = 100;
    public float currentHealth;
    public float moveSpeed = 2f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;

    private float attackTimer = 0f;
    private bool isDead = false;
    private bool playerInRange = false;

    private SpriteRenderer sr;   // ← we flip only the sprite, not the whole Boss

    void Start()
    {
        currentHealth = maxHealth;
        anim.SetBool("walk", false);

        sr = GetComponent<SpriteRenderer>(); // get sprite renderer on Boss root
    }

    void Update()
    {
        if (isDead || player == null) return;

        attackTimer -= Time.deltaTime;

        // If player is not in detection zone → idle
        if (!playerInRange)
        {
            anim.SetBool("walk", false);
            return;
        }

        // Flip only the sprite
        FlipTowardsPlayer();

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            WalkTowardPlayer();
            return;
        }

        TryAttack();
    }

void FlipTowardsPlayer()
{
    if (player.position.x > transform.position.x)
        sr.flipX = true;    // player on right → flip sprite
    else
        sr.flipX = false;   // player on left  → normal
}

    void WalkTowardPlayer()
    {
        anim.SetBool("walk", true);

        Vector2 target = new Vector2(player.position.x, transform.position.y);
        Vector2 newPos = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        rb.MovePosition(newPos);
    }

    void TryAttack()
    {
        anim.SetBool("walk", false);

        if (attackTimer <= 0)
        {
            anim.SetTrigger("Melee");
            attackTimer = attackCooldown;
        }
    }

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
        anim.SetBool("walk", false);

        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;

        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;

        Destroy(gameObject, 2f);
    }

    // ------------------------------
    // DETECTION ZONE TRIGGER LOGIC
    // ------------------------------
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            anim.SetBool("walk", false);
        }
    }
}
