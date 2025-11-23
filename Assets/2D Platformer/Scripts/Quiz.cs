using UnityEngine;

public class Quiz : MonoBehaviour
{
    public AudioClip deathClip;

    public void die()
    {
        // Play death sound
        AudioSource.PlayClipAtPoint(deathClip, Camera.main.transform.position);

        // Find player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            // Get PlayerController component
            Platformer.PlayerController controller = player.GetComponent<Platformer.PlayerController>();

            if (controller != null)
            {
                // Set death state before destroying player
                controller.deathState = true;
            }

            Debug.Log("Player has died.");
        }
    }
}
