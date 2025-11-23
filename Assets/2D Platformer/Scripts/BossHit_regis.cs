using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossHit_regis : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
     if (collision.CompareTag("Player"))
        {
            
            Destroy(transform.root.gameObject);
        }
}
}
