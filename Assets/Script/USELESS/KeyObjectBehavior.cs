using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class KeyObjectBehavior : MonoBehaviour
{
    public GameObject spawnLoc;

    private bool canBeGrabbed = true;
    private bool canMove = false;
    private bool hasInitialized = true;
    private bool canMoveBack = false;

    private SpriteRenderer myRend;
    private GameObject mPlayer;
    private Rigidbody2D myRb;
    private Color orgColor;
    private GameManager myManager;

    void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag("Player");
        myRb = GetComponent<Rigidbody2D>();
        myRend = GetComponent<SpriteRenderer>();
        orgColor = myRend.color;
        myManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void FixedUpdate()
    {
        if (canBeGrabbed && mPlayer != null)
        {
            if (Vector2.Distance(transform.position, mPlayer.transform.position) <= 4.0f && 
                mPlayer.GetComponent<PlayerBehavior>().canGrab)
            {
                canMove = true;
                hasInitialized = false;
            }
            else
            {
                canMove = false;
                myRb.mass = 2f;
                myRb.gravityScale = 3f;
                myRb.freezeRotation = false;
                if (!hasInitialized)
                {
                    myRb.velocity = Vector2.zero;
                    hasInitialized = true;
                    /*Debug.Log((transform.position - mPlayer.transform.position) * 100f);*/
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
            if (mPlayer != null)
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), mPlayer.GetComponent<Collider2D>(), false);
            foreach (GameObject box in GameObject.FindGameObjectsWithTag("Box"))
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), box.gameObject.GetComponent<Collider2D>(), false);
            }
        }

        if (canMoveBack)
        {
            transform.position = Vector2.Lerp(transform.position, spawnLoc.transform.position, 10f * Time.fixedDeltaTime);
        }
        
        if (Vector2.Distance(transform.position, spawnLoc.transform.position) <= 0.1f)
        {
            canMoveBack = false;
            canBeGrabbed = true;
            myRend.color = orgColor;
            myRb.simulated = true;
            myRb.velocity = Vector2.zero;
            GetComponent<Light2D>().intensity = 0.4f;
            myManager.canSpawnEnemy = true;
        }
    }

    public void ChangeState()
    {
        canBeGrabbed = false;
        canMove = false;
        myRend.color = Color.grey;
        GetComponent<Light2D>().intensity = 0f;
    }

    private void MoveTowardsPlayer()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), mPlayer.gameObject.GetComponent<Collider2D>());
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

    public void StartMovingBackToSpawn()
    {
        canMoveBack = true;
        myRb.simulated = false;
    }
}
