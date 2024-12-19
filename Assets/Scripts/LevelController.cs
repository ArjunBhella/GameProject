using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class LevelController : MonoBehaviour
{
    public TextMeshProUGUI points;
    public TextMeshProUGUI timer;

    public Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        ShowPoints();
        
    }

    // Update is called once per frame
    void Update()
    {
        ShowTime();
        ShowPoints();
        ShowHealth();
        
    }

    public void ShowPoints()
    {
        if (points != null)
        {
            points.text = "Fingers Collected: " + GameManager.Instance.score.ToString();
        }
    }
      public void ShowTime()
    {
        if (timer!=null)
        {
            timer.text = "Time left before Gojo is sealed: " + GameManager.Instance.UpdateTimerDisplay();
        }
    }
        public void ShowHealth()
    {
        if (healthBar!=null)
        {
            healthBar.fillAmount = (float)GameManager.Instance.currentHealth/(float)GameManager.Instance.maxHealth;
        }
    }


}
