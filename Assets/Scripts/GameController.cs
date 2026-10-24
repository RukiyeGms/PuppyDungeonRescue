using System.Collections;
using TMPro;
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

    [SerializeField] GameObject winPanel;
    [SerializeField] TextMeshProUGUI pickedText;
    [SerializeField] TextMeshProUGUI earnedText;
    [SerializeField] TextMeshProUGUI totalText;
    [SerializeField] GameObject coinLine;

    bool gameOver;

    int Coins;
    CoinUI coinUI;

    bool hasKey;
    [SerializeField] Image keyUIImage;
    [SerializeField] Image adventurerIconImage;
    [SerializeField] Image adventurerOnMapImage;

    [SerializeField] GameObject menuScreen;

    bool hasSheep;

    [SerializeField] Image room1;
    [SerializeField] Image room2;
    [SerializeField] Image room3;
    [SerializeField] Image room4;
    [SerializeField] Image room5;
    Image lastroom;

    private void Awake()
    {
        instance = this;
        coinUI = FindAnyObjectByType<CoinUI>();
    }
    void Start()
    {
        Coins = 0;
        hasKey = false;
        hasSheep = false;
        keyUIImage.enabled = false;
        adventurerIconImage.enabled = false;
        adventurerOnMapImage.enabled = true;
        winPanel.SetActive(false);
        lastroom = room1;
        ChangeMap(room1);

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
        instance.hasKey = false;
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

    public static bool AskSheep()
    {
        return instance.hasSheep;
    }

    public static void GetSheep()
    {
        instance.hasSheep = true;
        instance.adventurerIconImage.enabled = true;
        instance.adventurerOnMapImage.enabled = false;
    }

    public static void Win()
    {
        instance.winPanel.SetActive(true);
        instance.pickedText.text = $"Coins found: {instance.Coins}";
        instance.pickedText.gameObject.SetActive(false);

        instance.earnedText.text = "Payment received: 50";
        instance.earnedText.gameObject.SetActive(false);

        instance.totalText.text = $"Total earned: {instance.Coins + 50}";
        instance.totalText.gameObject.SetActive(false);
        instance.coinLine.SetActive(false);
        Time.timeScale = 0;

        instance.WinCoroutine();
    }

    void WinCoroutine()
    {
        StartCoroutine(WinScreen());
    }

    IEnumerator WinScreen()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        instance.pickedText.gameObject.SetActive(true );
        SoundEffectManager.Play("Boom");
        yield return new WaitForSecondsRealtime(1.5f);
        instance.earnedText.gameObject.SetActive(true);
        SoundEffectManager.Play("Boom");
        yield return new WaitForSecondsRealtime(1.5f);
        instance.totalText.gameObject.SetActive(true);
        instance.coinLine.SetActive(true);
        SoundEffectManager.Play("Boom");

    }

    
    public static void ChangeMap(Image roomImage)
    {

        Color c = instance.lastroom.color;
        c.a = 0.4f;
        instance.lastroom.color = c;

        c.a = 0.9f;
        roomImage.color = c;

        instance.lastroom = roomImage;
    }

}
