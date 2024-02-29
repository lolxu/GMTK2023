using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private float timerAmount = 100.0f;
    private float slowdownFactor = 0.05f;
    public GameObject enemy;
    private DoorLogicController doorLogic;
    private bool doorIsMoving = false;
    
    private GameObject[] keys;

    [Header("Game Logic Settings")]
    public bool canTransition = false;
    public bool needsRestart = false;
    public bool levelFinished = false;
    public bool canSpawnEnemy = true;

    [Header("Sound Settings")]
    public AudioClip doorOpen;
    public AudioClip death;
    private AudioSource mySource;
    private bool isPlayingSound = false;

    public bool isDoingSlowMotion = false;

    /*[Header("UI Settings")]*/
    /*public Image currentIcon;*/

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("DoorLogic"))
        {
            doorLogic = GameObject.FindGameObjectWithTag("DoorLogic").GetComponent<DoorLogicController>();
        }
        
        keys = GameObject.FindGameObjectsWithTag("Key");
        mySource = GetComponent<AudioSource>();
        /*FindObjectOfType<TimerBehavior>().canDecreaseTime = true;*/
        /*currentIcon.color = Color.white;*/
    }

    
    void Update()
    {
        MenuControls();

        if (levelFinished)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject e in enemies){
                if (e.GetComponent<EnemyBehavior>())
                {
                    e.GetComponent<EnemyBehavior>().Die();
                }
            }
            StartCoroutine(FinishProcedure());
        }
        else if (FindObjectOfType<PlayerBehavior>() == null)
        {
            if (!isPlayingSound)
            {
                mySource.clip = death;
                mySource.Play();
                isPlayingSound = true;
            }
            needsRestart = true;
            
        }
    }

    void MenuControls()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Debug special keys
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadScene(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SceneManager.LoadScene(4);
        }*/


    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        isDoingSlowMotion = true;
    }

    public void RevertToNormal()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
        isDoingSlowMotion = false;
    }

    IEnumerator FinishProcedure()
    {
        yield return new WaitForSeconds(3f);
        canTransition = true;
    }

}
