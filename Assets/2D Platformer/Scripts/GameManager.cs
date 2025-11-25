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

        private bool isRespawning = false;

        // ---------------------------------------------
        //           SINGLETON + PERSISTENT
        // ---------------------------------------------
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

        // ---------------------------------------------
        //                UPDATE LOOP
        // ---------------------------------------------
        void Update()
        {
            if (coinText != null)
                coinText.text = coinsCounter.ToString();

            if (!isRespawning && player != null && player.deathState)
            {
                StartCoroutine(DeathProcess());
                player.deathState = false;
            }
        }

        // ---------------------------------------------
        //          DEATH + RESPAWN SEQUENCE
        // ---------------------------------------------
        IEnumerator DeathProcess()
        {
            isRespawning = true;

            yield return new WaitForSeconds(0.05f);

            DisablePlayerComponents();

            // Spawn death animation prefab
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

            isRespawning = false;
        }

        // ---------------------------------------------
        //            DISABLE PLAYER (NOT GAMEOBJECT)
        // ---------------------------------------------
        void DisablePlayerComponents()
        {
            // Disable movement
            player.enabled = false;

            // Disable animator
            Animator anim = playerGameObject.GetComponent<Animator>();
            if (anim != null)
                anim.enabled = false;

            // Hide sprite
            SpriteRenderer sr = playerGameObject.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.enabled = false;

            // Disable collider
            Collider2D col = playerGameObject.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;

            // Reset velocity
            Rigidbody2D rb = playerGameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
        }

        // ---------------------------------------------
        //                RESPAWN LOGIC
        // ---------------------------------------------
        public void RespawnPlayer()
        {
            if (player == null)
                player = FindObjectOfType<PlayerController>();

            if (player == null)
            {
                Debug.LogError("GameManager: NO PLAYER FOUND TO RESPAWN!");
                return;
            }

            // Move player to spawn point
            player.MoveToSpawn();

            // Re-enable everything
            EnablePlayerComponents();
        }

        // ---------------------------------------------
        //            ENABLE PLAYER COMPONENTS
        // ---------------------------------------------
        void EnablePlayerComponents()
        {
            // Movement
            player.enabled = true;

            // Animator
            Animator anim = playerGameObject.GetComponent<Animator>();
            if (anim != null)
                anim.enabled = true;

            // Sprite
            SpriteRenderer sr = playerGameObject.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.enabled = true;

            // Collider
            Collider2D col = playerGameObject.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = true;
        }
    }
}
