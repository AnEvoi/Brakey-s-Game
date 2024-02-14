using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("--Player Movement--")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 5f;
    private bool facingRight = true;

    [Header("--Jump--")]
    [SerializeField] private float jumpForce = 10f;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("--Dash--")]
    [SerializeField] private float dashCool = .5f, dashCounter;
    private bool isDashed;
    private bool isGrounded;

    [Header("--Meele--")]
    [SerializeField] private float damage = 1f;
    [SerializeField] private float knockbackForce;
    public Animator meeleAnim;
    bool isAttacked = false;

    [Header("--Health--")]
    [SerializeField] private Healthbar healthbar;
    [SerializeField] private float _currentHealth;
    private float _maxHealth = 3f;

    [Header("--Others--")]
    Rigidbody2D rb;
    public Collider2D sword;

    void Start() 
    {
        _currentHealth = _maxHealth;
        healthbar.UpdateHealthBar(_maxHealth,_currentHealth);

        sword.enabled = false;
        sword = GameObject.FindGameObjectWithTag("Sword").GetComponent<Collider2D>();
        meeleAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        // Handle horizontal movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f);
        if (!isDashed)
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        // Flips the Sprite
        if(horizontalInput < 0 && facingRight)
            Flip();

        if(horizontalInput > 0 && !facingRight)
            Flip();

        #region Jump
        // Handle jumping
        if(isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {       
            Jump();
            jumpBufferCounter = 0f;
        }
        
        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        #endregion

        #region Dash
        // Dash
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDashed)
        {
            isDashed = true;
            dashCounter = dashCool;
        }
        #endregion

        #region Attack 
        //Attack
        if(Input.GetMouseButtonDown(0))
        {
            sword.enabled = true;
            meeleAnim.SetTrigger("Attack");
            isAttacked = true;
        }
        else if (Input.GetMouseButtonUp(0)){
            sword.enabled = false;
        }
        #endregion

        DashUpdate();
    }

    void Jump()
    {
        // Apply upward force for jumping
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        // Optional: Set animation parameter for jumping
        // Example: GetComponent<Animator>().SetTrigger("Jump");

        // Set grounded status to false to prevent double jumping
        isGrounded = false;
    }

    void DashUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f);
        if (isDashed && dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            transform.Translate(movement * dashSpeed * Time.deltaTime);

            if (dashCounter <= 0)
            {
                isDashed = false;
            }
        }
    }


    // Check if the player is grounded (you may need to customize this based on your game)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if(collision.gameObject.CompareTag("Enemy"))
        {
            _currentHealth -= Random.Range(0.5f,1f);

            if (_currentHealth <= 0)
            {
                GetComponent<PlayerController>().enabled = false;
                healthbar.UpdateHealthBar(_maxHealth, _currentHealth);
            }
            else
            {
                healthbar.UpdateHealthBar(_maxHealth, _currentHealth);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isAttacked == true)
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            Vector2 knockback = direction * knockbackForce;

            collision.GetComponent<Enemy>().TakeDamage(damage, knockback);
            Debug.Log("hit");
        }
    }

    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;

    }
}