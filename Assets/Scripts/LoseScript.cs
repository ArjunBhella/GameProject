using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreenMenu : MonoBehaviour
{
    public GameObject loseScreenPanel; // Reference to the lose screen panel

    // void Start()
    // {
    //     loseScreenPanel.SetActive(false);
    // }

    // public void ShowLoseScreen()
    // {
    //     loseScreenPanel.SetActive(true);
        
    // }

   
    public void GoToMainMenu()
    {
       
        SceneManager.LoadScene("MainMenu"); 
    }

  
    public void RestartLevel()
    {
        // Time.timeScale = 1f;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
         GameManager.Instance.RestartGame();
        SceneManager.LoadScene("demo_city_night"); 
    }
}
