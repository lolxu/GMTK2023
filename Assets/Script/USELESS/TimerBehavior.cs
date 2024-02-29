using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TimerBehavior : MonoBehaviour
{
    public float time = 200.0f;
    public float addTime = 15.0f;
    public bool canDecreaseTime = true;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (canDecreaseTime)
        {
            time -= Time.deltaTime;
            if (time <= 0f) time = 0f;
        }
        GameObject.FindGameObjectWithTag("TimerUI").GetComponent<Text>().text = time.ToString("f2");
    }

    public void AddTime()
    {
        time += addTime;
    }
}
