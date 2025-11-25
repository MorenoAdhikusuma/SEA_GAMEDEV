using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public int coinsCounter = 0;

        public GameObject playerGameObject;
        private PlayerController player;
        public GameObject deathPlayerPrefab;
        public Text coinText;

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            ReconnectPlayer();
        }

        void OnLevelWasLoaded(int level)
        {
            ReconnectPlayer();
        }

        void ReconnectPlayer()
        {
            player = FindObjectOfType<PlayerController>();

            if (player != null)
                playerGameObject = player.gameObject;
        }

        void Update()
        {
            if (coinText != null)
                coinText.text = coinsCounter.ToString();

            if (player != null && player.deathState == true)
            {
                StartCoroutine(DeathProcess());
                player.deathState = false;
            }
        }

        IEnumerator DeathProcess()
        {
            yield return new WaitForSeconds(0.05f);

            if (playerGameObject != null)
                playerGameObject.SetActive(false);

            GameObject deathPlayer = Instantiate(
                deathPlayerPrefab,
                playerGameObject.transform.position,
                playerGameObject.transform.rotation
            );

            deathPlayer.transform.localScale = playerGameObject.transform.localScale;

            yield return new WaitForSeconds(3);

            ReloadLevel();
        }

        private void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
