using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator anim;
    public Rigidbody2D rb;
    public Transform firePoint;
    public CircleCollider2D meleeHitbox;   // 🔥 your circle melee collider object
    private SpriteRenderer sr;             // sprite renderer of Boss

    [Header("Stats")]
    public float maxHealth = 100;
    public float currentHealth;
    public float moveSpeed = 2f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;

    private float attackTimer = 0f;
    private bool isDead = false;
    private bool playerInRange = false;
    public bool isAttacking = false;

    void Start()
    {
        currentHealth = maxHealth;
        anim.SetBool("walk", false);

        sr = GetComponent<SpriteRenderer>();

        // disable melee hitbox at start
        if (meleeHitbox != null)
            meleeHitbox.enabled = false;
    }

    void Update()
    {
        if (isDead || player == null) return;

        attackTimer -= Time.deltaTime;

        // No player detected = idle
        if (!playerInRange)
        {
            anim.SetBool("walk", false);
            return;
        }

        // Flip sprite to face player
        FlipTowardsPlayer();

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            WalkTowardPlayer();
            return;
        }

        TryAttack();
    }

    // ----------------------------------------------
    //              FLIP TOWARDS PLAYER
    // ----------------------------------------------
    void FlipTowardsPlayer()
    {
        if (player.position.x > transform.position.x)
            sr.flipX = true;     // facing right
        else
            sr.flipX = false;    // facing left
    }

    // ----------------------------------------------
    //                MOVEMENT
    // ----------------------------------------------
    void WalkTowardPlayer()
    {
        anim.SetBool("walk", true);

        Vector2 target = new Vector2(player.position.x, transform.position.y);
        Vector2 newPos = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        rb.MovePosition(newPos);
    }

    // ----------------------------------------------
    //            ATTACKING LOGIC
    // ----------------------------------------------
    void TryAttack()
    {
        anim.SetBool("walk", false);

        if (attackTimer <= 0)
        {
            isAttacking = true;
            anim.SetTrigger("Melee");
            attackTimer = attackCooldown;
        }
    }

    // called by Animation Event
    public void EnableMeleeHitbox()
    {
        if (meleeHitbox != null)
            meleeHitbox.enabled = true;
    }

    // called by Animation Event
    public void DisableMeleeHitbox()
    {
        if (meleeHitbox != null)
            meleeHitbox.enabled = false;

        isAttacking = false;
    }

    // ----------------------------------------------
    //                  DAMAGE
    // ----------------------------------------------
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

    // ----------------------------------------------
    //        DETECTION AREA TRIGGER
    // ----------------------------------------------
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
