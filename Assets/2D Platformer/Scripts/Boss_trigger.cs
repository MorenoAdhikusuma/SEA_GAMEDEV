using UnityEngine;

public class Boss_trigger : MonoBehaviour
{

    public GameObject boss;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
