using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    void Start()
    {
         gameOverPanel.SetActive(true);
        if (GameManager.Instance)
        {
            scoreText.text = "Score: " + GameManager.Instance.score.ToString();
        } 
    }

  public void RestartGame()
    {
        GameManager.Instance.RestartGame();
        SceneManager.LoadScene(0);
    }

}
