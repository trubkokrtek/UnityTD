using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour
{
    [Header("Money")]
    public int startingMoney = 500;
    public static int Money;
    public TextMeshProUGUI MoneyCounter;

    [Header("Lives")]
    
    public int startingLives = 20;
    public TextMeshProUGUI LivesCounter;
    public static bool isGameOver = false;
    public Pause Pause;

    [Header("Canvas")]
    public GameObject GameOver;
    public GameObject Shop;
    public GameObject pauseMenu;
    public GameObject won;

    public static int Lives;
    void Start()
    {
        Pause = this.GetComponent<Pause>();
        GameOver.SetActive(false);
        Shop.SetActive(true);
        pauseMenu.SetActive(false);
        Money = startingMoney;
        Lives = startingLives;
    }

    void Update()
    {
        if (isGameOver)
        {
            return;
        }
        MoneyCounter.text = "$" + Money.ToString();
        LivesCounter.text = Lives.ToString() + " LIVES";
        if (Lives <= 0)
        {
            GameOver.SetActive(true);
            Time.timeScale = 0f;
            Shop.SetActive(false);
            isGameOver = true;
        }
    }
    public void Retry()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }
    public void pauseGame()
    {
        pauseMenu.SetActive(true);
        Shop.SetActive(false);
    }
    public void Continue()
    {
        pauseMenu.SetActive(false);
        Shop.SetActive(true);
        
    }
    public void pressContinue()
    {
        this.GetComponent<Pause>().Unpause();
    }
    public void Win()
    {
        won.SetActive(true);
        Time.timeScale = 0f;
        Shop.SetActive(false);
        isGameOver = true;
    }
}
