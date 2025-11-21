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

    // Update is called once per frame
    void Update()
    {
        
    }
}
