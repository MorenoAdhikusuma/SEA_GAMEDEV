using UnityEngine;
using UnityEngine.UI;

public class Boss_health : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Image healthBar;
    public CanvasGroup healthBarCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBarCanvas.alpha = 0f;
        }
    }

    public void HealthBarShow()
    {
        if (healthBar != null)
        {
            healthBarCanvas.alpha = 1f;
        }
    }

    public void HealthBarHide()
    {
        if (healthBar != null)
        {
            healthBarCanvas.alpha = 0f;
        }
    }

    // Call this whenever boss takes damage
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // Clamp to prevent negative values
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update UI bar
        if (healthBar != null)
            healthBar.fillAmount = (float)currentHealth / (float)maxHealth;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Boss defeated!");
        gameObject.SetActive(false);
        HealthBarHide();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
