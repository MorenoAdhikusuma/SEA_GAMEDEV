using UnityEngine;

public class Boss_trigger : MonoBehaviour
{

    public GameObject boss;
    private Boss_health bossHealth;

    private void Start()
    {
        bossHealth = boss.GetComponent<Boss_health>();
        if(boss != null)
        {
            boss.SetActive(false);
        }
        //Hide Boss dulu di awal game
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Boss Triggered");
            boss.SetActive(true);
            bossHealth.HealthBarShow();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Boss Trigger Exit");
            bossHealth.HealthBarHide();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
