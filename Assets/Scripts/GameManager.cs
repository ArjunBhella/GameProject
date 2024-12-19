using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    private int storedScore = 0;
    public int maxHealth = 100;
    public int currentHealth;
    public float gameDuration = 60f;
    
    private float timer;
    private AudioSource audioSource;
    public AudioClip backgroundMusic;
    public TextMeshProUGUI timerText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        timer = gameDuration; 
        currentHealth = maxHealth;
        if (backgroundMusic != null && audioSource != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void RestartGame()
    {
        score = 0;
        currentHealth = maxHealth;
        timer = gameDuration;
    }
    
    public void IncrementScore()
    {
        score += 1;
         if (score == 5) // Check if the score reaches 10
        {
             SceneManager.LoadScene("2ndLevel"); // Load the second scene
        }
    }

    public void StoreScore()
    {
        storedScore = score;
    }

    public void ResetToPreviousScore()
    {
        score = storedScore;
    }

    public void DeincrementScore()
    {
        ResetToPreviousScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNewScene()
    {
        SceneManager.LoadScene("VictoryScene");
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimerDisplay();

            if (timer <= 0)
            {
                GameOver();
            }
        }
    }

    public String UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
         String timercheck = string.Format("{0:00}:{1:00}", minutes, seconds);

        timerText.text = timercheck;

        return timercheck;
    }

    void GameOver()
    {
        timer = 0;
        UpdateTimerDisplay();
        SceneManager.LoadScene("GameOverScene");
    }
}
