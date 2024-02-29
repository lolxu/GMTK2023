using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class MovablePlatformBehavior : MonoBehaviour
{
    [SerializeField] Color newColor;
    public bool canMove = true;
    private AudioSource myAudio;

    private void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        myAudio.pitch = Random.Range(0.75f, 1.0f);
        Rigidbody2D otherRb = other.gameObject.GetComponent<Rigidbody2D>();
        if (otherRb != null)
        {
            myAudio.volume = (GetComponent<Rigidbody2D>().velocity.sqrMagnitude + otherRb.velocity.sqrMagnitude);
        }
        
        myAudio.PlayOneShot(myAudio.clip);
        
        if (other.gameObject.CompareTag("Player"))
        {
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
