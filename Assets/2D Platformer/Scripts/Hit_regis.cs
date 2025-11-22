using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hit_regis : MonoBehaviour
{
    public Boss_health bossHealth;

    void Start()
    {
        // Auto-find the Boss_health script in the scene
        if (bossHealth == null)
        {
            bossHealth = FindObjectOfType<Boss_health>();
            if (bossHealth == null)
                Debug.LogError("Boss_health NOT FOUND in scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            Debug.Log("Hit registered on enemy!");
        }

        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
            Debug.Log("Projectile destroyed on ground hit.");
        }

        if (collision.CompareTag("Boss"))
        {
            if (bossHealth == null)
            {
                Debug.LogError("BossHealth reference is missing!");
            }
            else
            {
                bossHealth.TakeDamage(10);
            }

            Destroy(gameObject); // Destroy projectile after hitting boss
            Debug.Log("Hit registered on boss!");
        }
    }
}
