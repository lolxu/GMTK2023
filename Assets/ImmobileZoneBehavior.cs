using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmobileZoneBehavior : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    private PlayerBehavior myPlayer;
    private void Start()
    {
        myPlayer = FindObjectOfType<PlayerBehavior>();
    }

    void FixedUpdate()
    {
        if (FindObjectOfType<PlayerBehavior>() != null)
        {
            transform.position = myPlayer.gameObject.transform.position + offset;
        }
        
    }
}
