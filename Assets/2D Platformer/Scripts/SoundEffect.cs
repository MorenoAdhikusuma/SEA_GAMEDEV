using UnityEngine;

public class Coin : MonoBehaviour
{
    public AudioClip collectSound;   // Sound effect
    public float volume = 1f;        // Volume

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Play sound at camera position so it's always heard
            AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position, volume);

            Destroy(gameObject); // Remove the coin after collecting
        }
    }
}
