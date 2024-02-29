using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillerBehavior : MonoBehaviour
{
    public AudioClip deathSound;
    private AudioSource mySource;
    private void Start()
    {
        mySource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            mySource.clip = deathSound;
            mySource.pitch = Random.Range(0.8f, 1.0f);
            mySource.Play();
            other.gameObject.GetComponent<EnemyBehavior>().Die();
        }
    }
}
