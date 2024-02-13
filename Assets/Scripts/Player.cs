using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Speed")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Dash")]
    [SerializeField] private float dashCool = .5f, dashCounter;
    [SerializeField] private bool isDashed;
    [SerializeField] private bool isGrounded;

    [Header("Meele")]
    public Animator anim;
    private float timeUntilMeele;
    [SerializeField] private float meeleSpeed;
    bool isAttacked = false;

    private void Start() 
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        // Handle horizontal movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f);
        if (!isDashed)
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
        
        // Dash
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDashed)
        {
            isDashed = true;
            dashCounter = dashCool;
        }

        //Attack
        if(Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
            isAttacked = true;
            
        }

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
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isAttacked == true)
        {
            collision.GetComponent<Enemy>().TakeDamage(1);
            Debug.Log("hit");
        }
    }
}