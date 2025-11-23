using UnityEngine;

public class Quiz : MonoBehaviour
{
public AudioClip deathClip;

public void die()
{
    // putar suara dulu
    AudioSource.PlayClipAtPoint(deathClip, Camera.main.transform.position);
    Destroy(GameObject.FindGameObjectWithTag("Player"));

    // baru hancurkan player
    GameObject player = GameObject.FindGameObjectWithTag("Player");

    Debug.Log("Player has died.");
}

}
