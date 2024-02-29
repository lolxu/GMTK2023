using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public Image deathScreen;
    public float transitionTime = 1f;
    public Image congratsScreen;
    public Text congratsText;

    void Update()
    {
        if (FindObjectOfType<GameManager>().canTransition)
        {
            LoadNextLevel();
        }
        if (FindObjectOfType<GameManager>().needsRestart)
        {
            transition.SetTrigger("Start");
            Color tmpColor = deathScreen.color;
            tmpColor.a += Time.deltaTime * 2f;
            deathScreen.color = tmpColor;
        }
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 8)
        {
            StartCoroutine(LoadLevel(0));
        }
        else
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
        
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
