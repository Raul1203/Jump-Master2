using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public delegate void GameWon();
    public event GameWon GameWonEvent;

    public float moveSpeed = 2f;
    Rigidbody2D rb;
    SpriteRenderer renderer;
    Animator animator;
    bool isMoving = false;
    bool izGrounded = false;
    bool jumpRegister = false;
    Collider2D collider;
    float jumpPower = 0;
    bool isJumping = false;
    float move = 0f;
    bool jump = false;
    bool gameFinished = false;
    [SerializeField]
    LayerMask GroundMask;
    GameManager gameManger;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        gameManger = GameManager.instance;
        gameManger.GameOverEvent += OnGameOverEvent;
    }

    private void OnDestroy()
    {
        gameManger.GameOverEvent -= OnGameOverEvent;
    }
    // Update is called once per frame
    void Update()
    {
        if(gameFinished)
        {
            return;
        }

        bool wasGrounded = izGrounded;
        izGrounded = IsGrounded();

        if (izGrounded && isJumping)
            jumpRegister = false;

        isJumping = !izGrounded;

        if (!wasGrounded && izGrounded)
        {
            rb.velocity = Vector3.zero;
        }

        jump = Input.GetButton("Jump");

        if (jump && izGrounded)
        {
            isMoving = false;
            jumpPower += 10f;
            if (jumpPower > 560)
                jumpPower = 560;
        }
        else
        {
            move = Input.GetAxisRaw("Horizontal");
            if (move < 0)
            {
                renderer.flipX = true;
            }
            if (move > 0)
            {
                renderer.flipX = false;
            }

            if (jumpPower == 0)
            {
                isMoving = move != 0 && !jumpRegister && !isJumping;

            }
            else
                jumpRegister = true;
        }
    }

    private void FixedUpdate()
    {
        if(gameFinished)
        {
            return;
        }
        if (jumpRegister && jumpPower != 0)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Force);
            jumpPower = 0;
        }
        if (move != 0 && !jump)
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void LateUpdate()
    {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isJumping", isJumping);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(collider.bounds.center, new Vector2(collider.bounds.size.x - 0.1f, collider.bounds.size.y), 0, Vector2.down, 0.1f, GroundMask);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name=="finish")
        {
            GameWonEvent?.Invoke();
        }
    }
    
    private void OnGameOverEvent()
    {
        gameFinished = true;
        isMoving = false;
        isJumping = false;
        rb.velocity = new Vector2(0, 0);
    }
}