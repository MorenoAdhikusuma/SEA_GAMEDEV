using UnityEngine;

public class BossAI : MonoBehaviour
{
    public float moveSpeed = 2f; 
    public LayerMask ground;
    public LayerMask wall;

    private Rigidbody2D rigidbody; 
    public Collider2D triggerCollider;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, rigidbody.linearVelocity.y + moveSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if(!triggerCollider.IsTouchingLayers(ground) || triggerCollider.IsTouchingLayers(wall))
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        moveSpeed *= -1;
    }
}
