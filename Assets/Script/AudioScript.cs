using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    private AudioSource mySource;
    private static AudioScript instance = null;
    public static AudioScript Instance { get { return Instance; } }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }
        mySource = GetComponent<AudioSource>();
        mySource.Play();
        DontDestroyOnLoad(this);
    }
}
