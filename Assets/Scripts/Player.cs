using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 5;
    private Rigidbody2D rb2D;
    private float move;
    public float jumpForce = 8;
    private bool isGrounded;
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;
    private Animator animator;

    //doble salto
    public int maxJumps = 2;
    private int jumpsLeft;

    //coin
    private int coins;
    public TMP_Text textCoins;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpsLeft = maxJumps;
    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        rb2D.linearVelocity = new Vector2(move * speed, rb2D.linearVelocity.y);

        if (move != 0)
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);

        if (Input.GetButtonDown("Jump") && jumpsLeft > 0)
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
            jumpsLeft--;
        }

        animator.SetFloat("Speed", Mathf.Abs(move));
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (isGrounded && rb2D.linearVelocity.y <= 0.01f)
        {
            // al tocar el suelo se recuperan los saltos
            jumpsLeft = maxJumps;
        }
        else if (!isGrounded && jumpsLeft == maxJumps)
        {
            // si te caes de una plataforma sin saltar, pierdes el primer salto
            jumpsLeft = maxJumps - 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            coins++;
            textCoins.text = coins.ToString();
        }
        if (collision.transform.CompareTag("Spikes"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}