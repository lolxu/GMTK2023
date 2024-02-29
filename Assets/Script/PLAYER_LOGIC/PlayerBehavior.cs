using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    public bool isAlive = true;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 12f;
    private float horizontalMovement;
    private Rigidbody2D myRb;
    public float jumpForce = 20f;
    private bool hasJumped = false;
    public bool onGround = false;

    [Header("Collider Settings")]
    public Collider2D floorCollider;
    public Collider2D bodyCollider;
    public ContactFilter2D floorFilter;
    public GameObject deathParticles;
    public GameObject colParticles;
    public ParticleSystem dustParticles;
    public ParticleSystem walkParticles;
    public bool canGrab = false;
    private SpriteRenderer myRend;
    private Color orgColor;

    public Collider2D jumpDetector;

    private bool isFacingLeft = false;
    private Vector2 facingLeft;
    private Vector2 facingRight;
    private int lives = 3;

    [Header("Audio Settings")]
    public AudioClip[] hitSounds;
    public AudioClip jumpSound;
    [SerializeField] AudioClip powerupSound;
    [SerializeField] AudioClip debuffSound;
    private AudioSource mySource;

    private bool canJump = false;
    
    private float timeScale = 0.5f;
    float startMass;
    float startGravityScale;
    private bool slowMoBool = true;
    
    [Header("UI settings")]
    [SerializeField] private Image[] myHealthIcons;

    public bool canDamage = true;
    
    private bool onWall = false;
    private float myDir = 1.0f;
    private bool canTurn = true;

    private void Awake()
    {
        facingLeft = new Vector2(transform.localScale.x, transform.localScale.y);
        facingRight = new Vector2(-transform.localScale.x, transform.localScale.y);
        myRb = GetComponent<Rigidbody2D>();
        myRend = GetComponent<SpriteRenderer>();
        orgColor = myRend.color;
        transform.localScale = facingRight;
        mySource = GetComponent<AudioSource>();
        
        startGravityScale = myRb.gravityScale;
        startMass = myRb.mass;
        
    }
    
    private void Update()
    {
        
        if (isAlive && !FindObjectOfType<GameManager>().levelFinished)
        {
            horizontalMovement = movementSpeed * myDir;
            onGround = floorCollider.IsTouching(floorFilter);
            onWall = bodyCollider.IsTouching(floorFilter);

            if (!hasJumped && canJump && onGround)
            {
                hasJumped = true;
                canJump = false;
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                slowMoBool = true;
                movementSpeed *= 0.5f;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                slowMoBool = false;
                movementSpeed /= 0.5f;
            }

            if (onWall && canTurn && onGround)
            {
                myDir *= -1;
                canTurn = false;
            }

            if (!onWall)
            {
                canTurn = true;
            }
        }
        
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {

            myRb.velocity = new Vector2(horizontalMovement * movementSpeed, myRb.velocity.y);

            if (horizontalMovement != 0 && onGround)
            {
                CreateWalkingDust();
            }

            if (horizontalMovement > 0 && isFacingLeft)
            {
                isFacingLeft = false;
                Flip();
            }
            if (horizontalMovement < 0 && !isFacingLeft)
            {
                isFacingLeft = true;
                Flip();
            }

            if (hasJumped)
            {
                Debug.Log("HERE");
                hasJumped = false;
                myRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                CreateDust();
                mySource.clip = jumpSound;
                mySource.pitch = Random.Range(0.6f, 1.0f);
                mySource.Play();
            }
        }
    }

    private void Flip()
    {
        if (isFacingLeft)
        {
            transform.localScale = facingLeft;
            transform.GetChild(2).transform.localScale = new Vector2(1f, 1f);
        }
        if (!isFacingLeft)
        {
            transform.localScale = facingRight;
            transform.GetChild(2).transform.localScale = new Vector2(-1f, 1f);
        }
        if (onGround)
            CreateDust();
    }

    public void Kill(Vector2 spawnLoc)
    {
        if (canDamage)
        {
            mySource.clip = hitSounds[Random.Range(0, hitSounds.Length)];
            mySource.Play();
            myRend.color = Color.white;
            FindObjectOfType<HitStop>().Stop(0.5f);
            
            lives--;
            myHealthIcons[lives].color = Color.gray;
            myRend.color = Color.gray;
            StartCoroutine(HitRoutine(spawnLoc));

            canDamage = false;
            StartCoroutine(InvincibleTime());

            if (lives == 0)
            {
                isAlive = false;
                myRb.velocity = Vector2.zero;
                StartCoroutine(DeathRoutine());
            }
        }
    }

    IEnumerator InvincibleTime()
    {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("Can Damage");
        canDamage = true;
        myRend.color = orgColor;
    }

    public IEnumerator DeathRoutine()
    {
        while (Time.timeScale != 1.0f)
        {
            yield return null;
        }
        StartCoroutine(FindObjectOfType<CinemachineShake>().CamShake(5f, .5f));
        GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        particles.GetComponent<ParticleSystem>().Play();
        myRb.simulated = false;
        myRend.enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSecondsRealtime(.5f);
        Destroy(gameObject);
    }

    IEnumerator HitRoutine(Vector2 spawnLoc)
    {
        while (Time.timeScale != 1.0f)
        {
            yield return null;
        }
        StartCoroutine(FindObjectOfType<CinemachineShake>().CamShake(5f, .5f));
        GameObject particles = Instantiate(colParticles, spawnLoc, Quaternion.identity);
        particles.GetComponent<ParticleSystem>().Play();
        /*myRend.color = orgColor;*/
    }

    void CreateDust()
    {
        dustParticles.Play();
    }

    void CreateWalkingDust()
    {
        walkParticles.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.CompareTag("DoorZone"))
        {
            DoorBehavior[] doors = FindObjectsOfType<DoorBehavior>();
            foreach (var door in doors)
            {
                door.CanMove();
            }
            FindObjectOfType<GameManager>().levelFinished = true;
            StartCoroutine(Deceleration());
        }
        else if (!collision.gameObject.CompareTag("ImmobileZone"))
        {
            canJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canJump = false;
    }

    IEnumerator Deceleration()
    {
        while (horizontalMovement >= 0.0f)
        {
            horizontalMovement -= 0.25f;
            yield return new WaitForSeconds(0.2f);
        }

        horizontalMovement = 0.0f;
    }
}
