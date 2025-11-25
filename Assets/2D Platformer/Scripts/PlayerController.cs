using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        private static PlayerController instance;

        public float movingSpeed;
        public float jumpForce;
        private float moveInput;
        private bool facingRight = false;
        [HideInInspector]
        public bool deathState = false;

        private bool isGrounded;
        public Transform groundCheck;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;

        public AudioSource jump;
        public AudioSource death;

        public Transform firePoint;
        public GameObject bulletPrefab;
        public float bulletSpeed = 10f;
        public float fireRate = 0.5f;
        private float nextFireTime = 0f;

        bool jumpPressed = false;
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

            MoveToSpawn(); // ensure correct position on scene start
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
        void Update()
        {
            if (DialogueManager.instance != null && DialogueManager.instance.isDialogueActive)
            {
                moveInput = 0;
                animator.SetInteger("playerState", 0);
                return;
            }

            // MOVEMENT
            if (Input.GetButton("Horizontal"))
            {
                moveInput = Input.GetAxis("Horizontal");
                Vector3 direction = transform.right * moveInput;

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    transform.position + direction,
                    movingSpeed * Time.deltaTime
                );

                animator.SetInteger("playerState", 1);
            }
            else
            {
                if (isGrounded)
                    animator.SetInteger("playerState", 0);
            }

            // JUMP
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                jumpPressed = true;

            if (!isGrounded)
                animator.SetInteger("playerState", 2);

            // FLIP
            if (!facingRight && moveInput > 0)
                Flip();
            else if (facingRight && moveInput < 0)
                Flip();

            // SHOOT
            if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {
                ProjectileShoot();
                nextFireTime = Time.time + fireRate;
            }
        }
        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
        }
        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                groundCheck.transform.position,
                0.2f
            );

            isGrounded = colliders.Length > 1;
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy" && deathState == false)
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
        private void ProjectileShoot()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            Vector2 direction = (mousePosition - transform.position).normalized;

            GameObject projectile = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * bulletSpeed;
        }

        void OnLevelWasLoaded(int level)
        {
            MoveToSpawn();
        }

        public void MoveToSpawn()
        {
            string spawnID = PlayerPrefs.GetString("SpawnID", "");

            if (spawnID == "")
                return;

            SpawnPoint[] points = FindObjectsOfType<SpawnPoint>();

            foreach (SpawnPoint p in points)
            {
                if (p.spawnID == spawnID)
                {
                    transform.position = p.transform.position;
                    break;
                }
            }
        }
    }
}
