using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameMaster : MonoBehaviour
{
    public float timeForEachTry;
    float timeCD;
    public Player player;
    public Vector3 playerSpawnPoint;
    public List<GameObject> lifesObjects = new List<GameObject>();
    public int lifes;
    int lifeIndex = 0;

    public Slider timer;

    List<GameObject> reachedLines = new List<GameObject>();
    int points = 0;
    public int pointsForEachLine;
    public TextMeshProUGUI pointsText;

    List<GameObject> reachedEndZones = new List<GameObject>();
    public GameObject endZoneReachedMark;

    public int sceneIndex;

    public GameObject winPopUp;
    public TextMeshProUGUI winPointsText;
    public GameObject losePopUp;
    public TextMeshProUGUI losePointsText;

    public AudioSource deathSound;
    public AudioSource loseSound;
    public AudioSource winSound;
    public AudioSource endZoneReachedSound;

    public float timeScale;

    public TMP_InputField consoleInput;
    public GameObject console;

    public GameObject startPanel;
    public GameObject menu;

    private void Start()
    {
        timeCD = timeForEachTry;
        timer.maxValue = timeForEachTry;
        timer.value = timeForEachTry;
        pointsText.text = "0";

        Time.timeScale = timeScale;

        if(startPanel != null)
        {
            startPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    void Update()
    {
        if(timeCD > 0)
        {
            timeCD -= Time.deltaTime;
            timer.value = timeCD;
        }else
        {
            Death();
        }

        if(Input.touchCount == 4 || (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Z)))
        {
            console.SetActive(true);
        }

    }

    public void Death()
    {
        lifes--;
        lifesObjects[lifeIndex].SetActive(false);
        lifeIndex++;
        deathSound.Play();
        if(lifes == 0)
        {
            Lose();
        }else
        {            
            player.hiddingRaftUnderPlayer = null;
            player.transform.SetParent(null);
            player.transform.position = playerSpawnPoint;
            timeCD = timeForEachTry;
            timer.value = timeCD;
            reachedLines.Clear();
        }
    }

    public void LineReached(GameObject line)
    {
        foreach(GameObject l in reachedLines)
        {
            if(l == line)
            {
                return;
            }
        }

        reachedLines.Add(line);

        points += pointsForEachLine;

        pointsText.text = points.ToString();
    }

    public void EndZoneReached(GameObject endZone)
    {
        foreach (GameObject e in reachedEndZones)
        {
            if (e == endZone)
            {
                return;
            }
        }

        reachedEndZones.Add(endZone);
        reachedLines.Clear();

        endZoneReachedSound.Play();

        timeCD = timeForEachTry;
        timer.value = timeCD;

        points += (int)timeCD;

        pointsText.text = points.ToString();

        player.transform.position = playerSpawnPoint;

        player.isOnEndZone = false;

        Instantiate(endZoneReachedMark, endZone.transform);

        if(reachedEndZones.Count == 3)
        {
            Win();
        }
    }

    void Lose()
    {
        losePopUp.SetActive(true);
        player.enabled = false;
        loseSound.Play();
        losePointsText.text = "Points: " + points.ToString();
        Time.timeScale = 0f;
    }

    void Win()
    {
        winPopUp.SetActive(true);
        player.enabled = false;
        winSound.Play();
        winPointsText.text = "Points: " + points.ToString();
        Time.timeScale = 0f;
    }

    public void NextLevel()
    {
        Time.timeScale = timeScale;
        SceneManager.LoadScene(sceneIndex + 1);       
    }

    public void RestartLevel()
    {
        Time.timeScale = timeScale;
        SceneManager.LoadScene(sceneIndex);
    }
    public void ToStart()
    {
        Time.timeScale = timeScale;
        SceneManager.LoadScene(0);
    }

    public void Commends()
    {
        if(consoleInput.text == "Win")
        {
            Win();
        }


        console.SetActive(false);
        consoleInput.text = "";
    }

    public void StartPlay()
    {
        Time.timeScale = timeScale;

        startPanel.SetActive(false);
    }

    public void ToMenu()
    {
        Time.timeScale = 0f;

        menu.SetActive(true);
    }

    public void ResumeGame()
    {
        if (!winPopUp.activeSelf && !losePopUp.activeSelf)
        {
            Time.timeScale = timeScale;
        }else
        {
            Time.timeScale = 0f;
        }
        menu.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
