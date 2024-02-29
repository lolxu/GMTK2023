using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class JumpPlatformBehavior : MonoBehaviour
{
    [SerializeField] private Color newColor;
    [SerializeField] private Vector2 bounceForce;
    public bool canMove = true;
    private Collider2D myCol;
    private bool hasTriggered = false;
    private AudioSource myAudio;
    private void Start()
    {
        myCol = GetComponent<BoxCollider2D>();
        myAudio = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        if (FindObjectOfType<PlayerBehavior>() != null)
        {
            if (myCol.IsTouching(FindObjectOfType<PlayerBehavior>().floorCollider) && !hasTriggered)
            {
                FindObjectOfType<PlayerBehavior>().GetComponent<Rigidbody2D>().AddForce(bounceForce);
                hasTriggered = true;
                SetColor();
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            myAudio.pitch = Random.Range(0.85f, 1.0f);
            myAudio.Play();
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().isKinematic = true;
            gameObject.layer = 3;
            SetColor();
            canMove = false;
        }
    }

    private void SetColor()
    {
        if (GetComponent<SpriteRenderer>())
        {
            GetComponent<SpriteRenderer>().color = newColor;
        }
        else
        {
            GetComponent<TextMeshPro>().color = newColor;
        }
        
    }
}
