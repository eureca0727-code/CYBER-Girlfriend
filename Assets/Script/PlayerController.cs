using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private int jumpCount = 0;
    private bool isGrounded = false;
    private float moveInput;

    private Rigidbody2D playerRigidbody;
    private bool isDead = false;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isDead) return;

        // 좌우 이동 입력 (구 Input System)
        moveInput = Input.GetAxisRaw("Horizontal");

        // 점프 (더블 점프 포함)
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            jumpCount++;
            playerRigidbody.linearVelocity = new Vector2(playerRigidbody.linearVelocity.x, 0f);
            playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // 짧은 점프
        if (Input.GetKeyUp(KeyCode.Space) && playerRigidbody.linearVelocity.y > 0f)
        {
            playerRigidbody.linearVelocity = new Vector2(
                playerRigidbody.linearVelocity.x,
                playerRigidbody.linearVelocity.y * 0.5f
            );
        }
    }
    private void FixedUpdate()
    {
        if (isDead) return;

        // 좌우 이동
        playerRigidbody.linearVelocity = new Vector2(moveInput * moveSpeed, playerRigidbody.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dead") && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        playerRigidbody.linearVelocity = Vector2.zero;
        Debug.Log("Player Died!");
    }
}
