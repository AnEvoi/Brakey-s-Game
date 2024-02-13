using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 3;
    [SerializeField] private float speed = 5f;

    public Animator anim;   
    GameObject player;

    public void TakeDamage(float damage)
    {
        health -= damage;
        anim.SetTrigger("Hurt");

        if (health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy has died");
        }
    }
    void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() 
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
