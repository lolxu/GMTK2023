using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTextBehavior : MonoBehaviour
{
    [SerializeField] private GameObject deathParticles;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject particles = Instantiate(deathParticles, other.contacts[0].point, Quaternion.identity);
            particles.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
        }
    }
}
