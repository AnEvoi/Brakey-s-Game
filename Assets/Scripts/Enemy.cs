using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 3;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float _knockbackForce = 5f;

    public Animator anim;  
    Rigidbody2D rb; 
    GameObject player;

    public void TakeDamage(float damage, Vector2 knockback)
    {
        health -= damage;
        rb.AddForce(knockback, ForceMode2D.Impulse);
        anim.SetTrigger("Hurt");

        if (health <= 0)
        {
            Points.pointCounter++;
            Destroy(gameObject);
            Debug.Log("Enemy has died");
        }
    }
    void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
