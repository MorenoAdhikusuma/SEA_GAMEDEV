using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public Transform leftPoint;
    public Transform rightPoint;

    private float direction = 1f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // detach patrol points from parent so they stay in place
        leftPoint.parent = null;
        rightPoint.parent = null;
    }

    private void Update()
    {
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

        if (direction > 0 && transform.position.x >= rightPoint.position.x)
            Flip();

        if (direction < 0 && transform.position.x <= leftPoint.position.x)
            Flip();
    }

    private void Flip()
    {
        direction *= -1;
        transform.localScale = new Vector3(direction, 1f, 1f);
    }
}
