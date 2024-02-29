using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public GameObject destination;
    private bool canMove = false;
    private AudioSource myAudio;
    private bool isPlayingAudio = false;

    private void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            MoveToLocation();
            if (!isPlayingAudio)
            {
                myAudio.Play();
                isPlayingAudio = true;
            }
        }

    }
    private void MoveToLocation()
    {
        transform.position = Vector2.Lerp(transform.position, destination.transform.position, 0.5f * Time.fixedDeltaTime);
    }

    public void CanMove()
    {
        canMove = true;
    }
    
    


}
