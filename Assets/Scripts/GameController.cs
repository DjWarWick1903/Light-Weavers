using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Text healthText;
    [SerializeField] private Text timerText;

    private GameObject player;
    private Player playerScript;

    private float secondsTimer = 0;
    private float minutesTimer = 5;
    private float timer = 0;

    private float playerHealth;
    private float playerCurrentHealth;

    private bool isPaused = false;

    void Start()
    {
        player = GameObject.Find("Player");

        playerScript = player.GetComponent<Player>();
        playerHealth = playerScript.GetPlayerHealth();

        timerText.text = minutesTimer.ToString() + ":00";
    }

    void Update()
    {
        if(isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        playerCurrentHealth = playerScript.GetPlayerHealth();

        int healthToText = (int)((playerCurrentHealth / playerHealth) * 100);
        healthText.text = healthToText.ToString() + "%";
        Timer();

        if(secondsTimer == 0 && minutesTimer == 0)
        {
            var levelController = GameObject.Find("LevelController");
            levelController.GetComponent<LevelController>().WinScreen();
        }

        if(Input.GetKeyUp(KeyCode.Escape))
        {
            isPaused = true;
        }

        if(Input.GetKeyUp(KeyCode.Escape) && isPaused)
        {
            isPaused = false;
        }
    }

    private void Timer()
    {
        if(secondsTimer == 0)
        {
            secondsTimer = 59;
            minutesTimer--;
        }

        if(secondsTimer < 10)
        {
            timerText.text = minutesTimer.ToString() + ":0" + secondsTimer.ToString(); 
        }
        else
        {
            timerText.text = minutesTimer.ToString() + ":" + secondsTimer.ToString();
        }

        if (timer > 1f)
        {
            secondsTimer--;
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
