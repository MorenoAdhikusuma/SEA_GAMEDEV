using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        private static PlayerController instance;

        public float movingSpeed;
        public float jumpForce;
        private float moveInput;
        private bool facingRight = true;
        [HideInInspector]
        public bool deathState = false;

        private bool isGrounded;
        public Transform groundCheck;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;

        public AudioSource jump;
        public AudioSource death;
        public AudioSource walk;

        public Transform firePoint;
        public GameObject bulletPrefab;
        public float bulletSpeed = 10f;
        public float fireRate = 0.5f;
        private float nextFireTime = 0f;

        bool jumpPressed = false;

        [SerializeField] private SpriteRenderer spriteRenderer;


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
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gameManager = FindObjectOfType<GameManager>();

            MoveToSpawn();
        }

        private void FixedUpdate()
        {
            if (DialogueManager.instance != null && DialogueManager.instance.isDialogueActive)
                return;

            CheckGround();

            if (jumpPressed && isGrounded)
            {
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                jump.Play();
                jumpPressed = false;
            }
        }

        // =====================================================
        // UPDATE
        // =====================================================
        void Update()
        {
            // Freeze movement during dialogue
            if (DialogueManager.instance != null && DialogueManager.instance.isDialogueActive)
            {
                moveInput = 0;
                animator.SetInteger("playerState", 0);
                return;
            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            moveInput = horizontal;

            // ---------------------------
            // MOVEMENT
            // ---------------------------
            if (horizontal != 0)
            {
                Vector3 direction = transform.right * horizontal;

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    transform.position + direction,
                    movingSpeed * Time.deltaTime
                );

                animator.SetInteger("playerState", 1);

                // WALK SOUND
                if (isGrounded && !walk.isPlaying)
                    walk.Play();
            }
            else
            {
                if (isGrounded)
                    animator.SetInteger("playerState", 0);

                // STOP WALKING SOUND
                if (walk.isPlaying)
                    walk.Stop();
            }

            // ---------------------------
            // FLIP
            // ---------------------------
            if (horizontal > 0 && !facingRight)
                Flip();
            else if (horizontal < 0 && facingRight)
                Flip();

            // ---------------------------
            // JUMP
            // ---------------------------
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                jumpPressed = true;

            if (!isGrounded)
                animator.SetInteger("playerState", 2);

            // ---------------------------
            // SHOOT
            // ---------------------------
            if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {
                ProjectileShoot();
                animator.SetTrigger("Shoot");
                nextFireTime = Time.time + fireRate;
            }
        }

        // =====================================================
        // FLIP
        // =====================================================
        private void Flip()
        {
            facingRight = !facingRight;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        // =====================================================
        // GROUND CHECK
        // =====================================================
        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                groundCheck.transform.position,
                0.2f
            );

            isGrounded = colliders.Length > 1;
        }

        // =====================================================
        // COLLISION
        // =====================================================
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy" && !deathState)
            {
                deathState = true;
                death.Play();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Coin")
            {
                gameManager.coinsCounter += 1;
                Destroy(other.gameObject);
            }
        }
        // =====================================================
        // SHOOT
        // =====================================================
        private void ProjectileShoot()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            Vector2 direction = (mousePosition - transform.position).normalized;

            GameObject projectile = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * bulletSpeed;
        }

        // =====================================================
        // SCENE LOAD
        // =====================================================
        void OnLevelWasLoaded(int level)
        {
            MoveToSpawn();
        }

        // =====================================================
        // SPAWN POINT
        // =====================================================
        public void MoveToSpawn()
        {
            string spawnID = PlayerPrefs.GetString("SpawnID", "");
            string currentScene = SceneManager.GetActiveScene().name;

            SpawnPoint[] points = FindObjectsOfType<SpawnPoint>();

            foreach (SpawnPoint p in points)
            {
                if (p.spawnID == spawnID && p.SceneName == currentScene)
                {
                    transform.position = p.transform.position;
                    return;
                }
            }

            foreach (SpawnPoint p in points)
            {
                if (p.SceneName == currentScene)
                {
                    transform.position = p.transform.position;
                    return;
                }
            }
        }
    }
}
