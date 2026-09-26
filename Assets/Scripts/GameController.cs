using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    //forgameover
    [SerializeField] GameObject puppyDownScreen;
    [SerializeField] FadeToBlack fadeToBlack;
    [SerializeField]Transform dungeonSpawnLocation;
    [SerializeField] GameObject puppy;
    void Start()
    {
        puppy.transform.position = dungeonSpawnLocation.position;
        PlayerHealth.OnPuppyDied += GameOver;
        puppyDownScreen.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(dungeonSpawnLocation.position, 0.5f );
    }

    void GameOver()
    {
        StartCoroutine(GameOverScreen());
    }
    IEnumerator GameOverScreen()
    {
        yield return StartCoroutine(fadeToBlack.FadetoBlack());
        fadeToBlack.gameObject.SetActive(false);
        puppyDownScreen.SetActive(true);
        //pauses game
        Time.timeScale = 0;
    }

    public void StartOver()
    {
        Time.timeScale = 1;
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
        
    }

    void OnDestroy()
    {
        PlayerHealth.OnPuppyDied -= GameOver;
    }


}
