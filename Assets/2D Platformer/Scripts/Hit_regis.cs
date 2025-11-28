using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hit_regis : MonoBehaviour
{
    public Boss_health bossHealth;
    public float autoDestroyTime = 1f;

    void Start()
    {
        Destroy(gameObject, autoDestroyTime);
          // Destroy projectile after a set time
        // Auto-find the Boss_health script in the scene
        if (bossHealth == null)
        {
            bossHealth = FindObjectOfType<Boss_health>();   
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(transform.root.gameObject);
        }

        if (collision.CompareTag("Ground"))
        {
           Destroy(transform.root.gameObject);
        }

        if (collision.CompareTag("Boss"))
        {
            bossHealth.TakeDamage(10);
            Destroy(transform.root.gameObject);
        }
    }
}
