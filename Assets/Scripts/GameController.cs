using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    bool gameOver;

    int Coins;
    CoinUI coinUI;

    bool hasKey;
    [SerializeField] Image keyUIImage;

    [SerializeField] GameObject menuScreen;

    private void Awake()
    {
        instance = this;
        coinUI = FindAnyObjectByType<CoinUI>();
    }
    void Start()
    {
        Coins = 0;
        hasKey = false;
        keyUIImage.enabled = false;

        gameOver = false;
        //disable menu

        puppy.transform.position = dungeonSpawnLocation.position;
        PlayerHealth.OnPuppyDied += GameOver;
        puppyDownScreen.SetActive(false);
        menuScreen.SetActive(false);
        BGSlider.SetActive(true);
        SFXSlider.SetActive(true);

        MusicManager.PlayBackground(true);
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
        gameOver = true;
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
        gameOver = false;
        
    }

    void OnDestroy()
    {
        PlayerHealth.OnPuppyDied -= GameOver;
    }


    public static void CoinUp(int amount)
    {
        instance.Coins += amount;
        SoundEffectManager.Play("Coin");
        instance.coinUI.UpdateCoinText();
    }

    public static int getCoin()
    {
        return instance.Coins;
    }

    public static void PickUpKey()
    {
        instance.hasKey = true;
        SoundEffectManager.Play("Key");
        instance.keyUIImage.enabled = true;
    }

    public static bool AskKey()
    {
        return instance.hasKey;
    }

    public static void UseKey()
    {
        //if there is a key unlock cage
        instance.keyUIImage.enabled = false;
    }

    public static void OpenMenu()
    {
        if (!instance.gameOver )
        {
            instance.menuScreen.SetActive(true);
            Time.timeScale = 0;
            //time pause
        }
    }

    public void CloseMenu()
    {
        menuScreen.SetActive(false);
        Time.timeScale = 1;
        //time cont
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
        MusicManager.PauseBackground();
        Time.timeScale = 1;
    }
}
