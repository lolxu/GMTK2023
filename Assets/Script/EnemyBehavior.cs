using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 10f;
    public GameObject EnemyDeathEffect;
    private GameObject player;
    private BoxCollider2D myCol;
    private bool isUp = false;
    private Rigidbody2D myRb;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myCol = GetComponent<BoxCollider2D>();
        myRb = GetComponent<Rigidbody2D>();
        
        isUp = true;
        myCol.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerBehavior>())
            {
                if (other.gameObject.GetComponent<PlayerBehavior>().canDamage)
                {
                    other.gameObject.GetComponent<PlayerBehavior>().Kill(other.contacts[0].point);
                    Die();
                }
            }
            else
            {
                Die();
            }
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerBehavior>())
            {
                if (other.gameObject.GetComponent<PlayerBehavior>().canDamage)
                {
                    other.gameObject.GetComponent<PlayerBehavior>().Kill(other.contacts[0].point);
                    Die();
                }
            }
            else
            {
                Die();
            }
        }
    }

    public void Die()
    {
        GameObject particles = Instantiate(EnemyDeathEffect, transform.position, Quaternion.identity);
        particles.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
    }

}
