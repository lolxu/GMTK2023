using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutBoundsTrigger : MonoBehaviour
{
    [SerializeField] private GameObject deathParticles;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject particles = Instantiate(deathParticles, other.transform.position, Quaternion.identity);
            particles.GetComponent<ParticleSystem>().Play();
            Destroy(other.gameObject);
        }
    }
}
