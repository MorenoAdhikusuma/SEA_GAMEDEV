using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        public int coinsCounter = 0;

        public GameObject playerGameObject;
        private PlayerController player;
        public GameObject deathPlayerPrefab;
        public Text coinText;

        // public AudioClip deathClip;
        // public AudioSource audioSource;

        void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        void Update()
        {
            coinText.text = coinsCounter.ToString();

            if (player.deathState == true)
            {
                StartCoroutine(DeathProcess());
                player.deathState = false;
            }
        }

        IEnumerator DeathProcess()
        {
            // Mainkan suara death dulu (agar tidak terputus)
            // audioSource.PlayOneShot(deathClip);

            yield return new WaitForSeconds(0.05f); // ini ngeplay yang mana bjir

            playerGameObject.SetActive(false);

            GameObject deathPlayer = Instantiate(deathPlayerPrefab, playerGameObject.transform.position, playerGameObject.transform.rotation);
            deathPlayer.transform.localScale = playerGameObject.transform.localScale;

            yield return new WaitForSeconds(3);
            ReloadLevel();
        }

        private void ReloadLevel()
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
