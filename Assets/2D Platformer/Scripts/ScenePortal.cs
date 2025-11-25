using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class ScenePortal : MonoBehaviour
    {
        public string targetScene;       // nama scene tujuan
        public string targetSpawnID;     // spawn point yang ingin dipakai di scene tujuan

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerPrefs.SetString("SpawnID", targetSpawnID);
                SceneManager.LoadScene(targetScene);
            }
        }
    }
}
