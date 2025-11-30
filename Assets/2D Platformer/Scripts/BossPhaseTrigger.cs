using UnityEngine;

public class BossPhaseTrigger : MonoBehaviour
{
    public BossAI bossAI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bossAI.damageLocked = false;
            Debug.Log("Boss lock removed. Damage allowed again!");
            Destroy(gameObject); // trigger hanya sekali
        }
    }
}
