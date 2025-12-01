using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//DIS IS DA MAGIC THAT MAKES DA GAME WORK
//IDK HOW BUT IT DOES, DONT TOUCH IT PLEASE
//IF YOU BREAK IT I WILL FIND YOU
//- LEMON
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

        public static bool Paused = false;
        public GameObject PauseMenuUI;

        private bool isRespawning = false;

        // ------------------------------------------------
        // SINGLETON
        // ------------------------------------------------
        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void Start()
        {
            ReconnectPlayer();
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ReconnectPlayer();
        }

        void ReconnectPlayer()
        {
            player = FindObjectOfType<PlayerController>();

            if (player != null)
                playerGameObject = player.gameObject;
        }

        // ------------------------------------------------
        // UPDATE LOOP
        // ------------------------------------------------
        void Update()
        {
            if (coinText != null)
                coinText.text = coinsCounter.ToString();

            if (!isRespawning && player != null && player.deathState)
            {
                StartCoroutine(DeathProcess());
                player.deathState = false;
            }

            // Pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Paused)
                    Resume();
                else
                    Pause();
            }
        }

        // ------------------------------------------------
        // DEATH + RESPAWN SEQUENCE
        // ------------------------------------------------
        IEnumerator DeathProcess()
        {
            isRespawning = true;

            yield return new WaitForSeconds(0.05f);

            DisablePlayerComponents();

            // Death animation prefab
            GameObject deathPlayer = null;
            if (deathPlayerPrefab != null)
            {
                deathPlayer = Instantiate(
                    deathPlayerPrefab,
                    player.transform.position,
                    player.transform.rotation
                );
                deathPlayer.transform.localScale = player.transform.localScale;
            }

            yield return new WaitForSeconds(2f);

            if (deathPlayer != null)
                Destroy(deathPlayer);

            Debug.Log("Respawning Player...");

            RespawnPlayer();
            player.MoveToSpawn();

            isRespawning = false;
        }

        // ------------------------------------------------
        // DISABLE PLAYER COMPONENTS
        // ------------------------------------------------
        void DisablePlayerComponents()
        {
            player.enabled = false;

            Animator anim = playerGameObject.GetComponent<Animator>();
            if (anim != null) anim.enabled = false;

            SpriteRenderer sr = playerGameObject.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;

            Rigidbody2D rb = playerGameObject.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

            Collider2D col = playerGameObject.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        }

        // ------------------------------------------------
        // RESPAWN
        // ------------------------------------------------
        public void RespawnPlayer()
        {
            if (player == null)
                player = FindObjectOfType<PlayerController>();

            EnablePlayerComponents();
        }

        // ------------------------------------------------
        // ENABLE PLAYER COMPONENTS
        // ------------------------------------------------
        void EnablePlayerComponents()
        {
            if (player == null) return;

            player.enabled = true;

            Animator anim = playerGameObject.GetComponent<Animator>();
            if (anim != null) anim.enabled = true;

            SpriteRenderer sr = playerGameObject.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = true;

            Collider2D col = playerGameObject.GetComponent<Collider2D>();
            if (col != null) col.enabled = true;
        }

        // ------------------------------------------------
        // PAUSE SYSTEM (NOW INSIDE CLASS)
        // ------------------------------------------------
        public void Resume()
        {
            PauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            Paused = false;
        }

        public void Pause()
        {
            PauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            Paused = true;
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene("PauseMenu");
            Time.timeScale = 1f;
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
