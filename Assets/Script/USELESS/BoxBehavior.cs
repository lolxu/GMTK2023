using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehavior : MonoBehaviour
{

    private GameObject mPlayer;
    private Rigidbody2D myRb;
    private GameManager myManager;

    private bool canBeGrabbed = true;
    private bool canMove = false;
    private bool hasInitialized = true;

    private void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag("Player");
        myRb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        

        if (canBeGrabbed && mPlayer != null)
        {
            if (Vector2.Distance(transform.position, mPlayer.transform.position) <= 2.0f &&
                mPlayer.GetComponent<PlayerBehavior>().canGrab)
            {
                canMove = true;
                hasInitialized = false;
            }
            else
            {
                canMove = false;
                myRb.mass = 4f;
                myRb.gravityScale = 3f;
                myRb.freezeRotation = false;
                if (!hasInitialized)
                {
                    myRb.velocity = Vector2.zero;
                    hasInitialized = true;
                    myRb.AddForce(mPlayer.GetComponent<Rigidbody2D>().velocity * 120f);
                }
            }
        }

        if (canMove && mPlayer != null)
        {
            MoveTowardsPlayer();
        }
        else
        {
            if (mPlayer == null)
            {
                myRb.mass = 2f;
                myRb.gravityScale = 3f;
                myRb.freezeRotation = false;
            }
            else if (mPlayer != null)
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), mPlayer.GetComponent<Collider2D>(), false);
            foreach(GameObject key in GameObject.FindGameObjectsWithTag("Key"))
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), key.gameObject.GetComponent<Collider2D>(),false);
            }
            foreach (GameObject box in GameObject.FindGameObjectsWithTag("Box"))
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), box.gameObject.GetComponent<Collider2D>(), false);
            }

        }
    }

    private void MoveTowardsPlayer()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), mPlayer.gameObject.GetComponent<Collider2D>());
        foreach (GameObject key in GameObject.FindGameObjectsWithTag("Key"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), key.gameObject.GetComponent<Collider2D>());
        }
        foreach (GameObject box in GameObject.FindGameObjectsWithTag("Box"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), box.gameObject.GetComponent<Collider2D>());
        }
        myRb.mass = 0.05f;
        myRb.gravityScale = 0.0f;
        myRb.freezeRotation = true;
        if (mPlayer != null)
            transform.position = Vector2.Lerp(transform.position, mPlayer.transform.position, 8f * Time.fixedDeltaTime);
    }
}
