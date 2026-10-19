using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    //forgameover
    [SerializeField] GameObject puppyDownScreen;
    [SerializeField] FadeToBlack fadeToBlack;
    [SerializeField] Transform dungeonSpawnLocation;
    [SerializeField] GameObject puppy;

    [SerializeField] GameObject SFXSlider;
    [SerializeField] GameObject BGSlider;

    int Coins;
    CoinUI coinUI;

    private void Awake()
    {
        instance = this;
        coinUI = FindAnyObjectByType<CoinUI>();
    }
    void Start()
    {
        Coins = 0;

        puppy.transform.position = dungeonSpawnLocation.position;
        PlayerHealth.OnPuppyDied += GameOver;
        puppyDownScreen.SetActive(false);
        BGSlider.SetActive(true);
        SFXSlider.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(dungeonSpawnLocation.position, 0.5f );
    }

    void GameOver()
    {
        StartCoroutine(GameOverScreen());
        MusicManager.PauseBackground();
        BGSlider.SetActive(false);
        SFXSlider.SetActive(false);
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
        Coins = 0;
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
        MusicManager.PlayBackground(true);
        
    }

    void OnDestroy()
    {
        PlayerHealth.OnPuppyDied -= GameOver;
    }


    public static void CoinUp()
    {
        instance.Coins++;
        SoundEffectManager.Play("Coin");
        instance.coinUI.UpdateCoinText();
    }

    public static int getCoin()
    {
        return instance.Coins;
    }
}
