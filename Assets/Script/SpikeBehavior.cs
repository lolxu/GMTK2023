using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehavior : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerBehavior>())
            {
                other.gameObject.GetComponent<PlayerBehavior>().Kill(other.contacts[0].point);
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
                }
            }
        }
    }

    private void Update()
    {
        if (FindObjectOfType<GameManager>().levelFinished)
        {
            GetComponent<Collider2D>().enabled = false;
            Color tmpColor = GetComponent<SpriteRenderer>().color;
            tmpColor.a -= Time.deltaTime * 2f;
            GetComponent<SpriteRenderer>().color = tmpColor;
        }
    }
}
