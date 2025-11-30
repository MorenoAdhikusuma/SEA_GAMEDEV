using UnityEngine;

public class Boss_melee_hitbox : MonoBehaviour
{
    public BossAI boss; // reference to the boss script

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!boss.isAttacking) return;  // only hit during attack animation

        if (other.CompareTag("Player"))
        {
            // Destroy the player or call damage function
            Destroy(other.gameObject);
        }
    }
}
