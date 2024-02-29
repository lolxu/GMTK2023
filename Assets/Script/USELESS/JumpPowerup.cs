using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPowerup : MonoBehaviour
{
    PlayerBehavior playerMove;
    [SerializeField] float jumpPowerChange = 5f;
    // Start is called before the first frame update
    void Start()
    {
        playerMove = FindObjectOfType<PlayerBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("triggered something");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("jump powerup collected");
            playerMove.jumpForce += jumpPowerChange;
            Destroy(gameObject);
        }
    }
}