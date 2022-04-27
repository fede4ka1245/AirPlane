using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class Game : MonoBehaviour
{
    int coins;
    int score;
    int bestScore;
    [SerializeField] Text coinsText;
    [SerializeField] Text scoreText;
    [SerializeField] Text bestScoreText;

    Save sv = new Save();

    [SerializeField] float time = 0.0001f;
    public float speed;
    [SerializeField] Plane plane;
    [SerializeField] public GameObject DeathPanel;
    [SerializeField] public GameObject StatPanel;
    [SerializeField] public GameObject WarningPanel;
    [SerializeField] public GameObject PausePanel;
    public float speedBoost;
    public float SpeedWithBoost;
    public click button;

    string planeName;

    [SerializeField] Plane[] PlanePrefabs;
    [SerializeField] Vector3 planeSpawnPosition;

    [SerializeField] GameObject sceneTransition; 


    void Start()
    {
        if (Advertisement.isSupported)
        {
            Advertisement.Initialize("4172732", false);
        }
        if (PlayerPrefs.HasKey("save"))
        {
            sv = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("save"));
            coins = sv.Coins;
            bestScore = sv.BestScore;
            planeName = sv.PlanesName;
        }
        SpawnPlane(planeName);
        ShowMainInfo();
        StatPanel.SetActive(true);


        Time.timeScale = 0;
    }

    private void FixedUpdate()
    {
        ScoreUpdate();
        SpeedUpdate();
    }

    private void SpawnPlane(string name)
    {
        foreach (var planePrefab in PlanePrefabs)
        {
            if (planePrefab.Name == name)
            {
                plane = Instantiate(planePrefab, planeSpawnPosition, Quaternion.identity);
                speed = plane.speed;
            }
        }
    }

    private void ScoreUpdate()
    {
        if (score > bestScore)
            bestScore = score;
        score += Convert.ToInt32(speed * time);
        ShowMainInfo();
    }

    private void SpeedUpdate()
    {
        if (SpeedWithBoost <= plane.MaxSpeed && !plane.IsCrush && speed + speedBoost <= plane.MaxSpeed)
        {
            speedBoost = score / 100;
            SpeedWithBoost = speed + speedBoost;
        }
    }

    private void ShowMainInfo()
    {
        coinsText.text = coins.ToString().PadLeft(5, '0');
        bestScoreText.text = "HI " + bestScore.ToString().PadLeft(5, '0');
        scoreText.text = score.ToString().PadLeft(5, '0');
    }

    public void CollectCoin(GameObject coin)
    {
        StartCoroutine(CollectCoinCoroutine(coin));
    }

    IEnumerator CollectCoinCoroutine(GameObject coin)
    {
        coins += 1;
        coin.SetActive(false);

        yield return new WaitForSeconds(2.5f);

        coin.SetActive(true);
    }

    public void Reborn()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("Rewarded_iOS");
            CloseAllPanel();
            plane.Reborn();
            PausePanel.SetActive(true);
        }
        else
        {
            WarningPanel.SetActive(true);
        }
    }

    public void CloseWarningPanel()
    {
        WarningPanel.SetActive(false);
    }    

    public void SaveAll()
    {
        sv.Coins = coins;
        sv.BestScore = bestScore;

        PlayerPrefs.SetString("save", JsonUtility.ToJson(sv));
    }

    public void CloseAllPanel()
    {
        DeathPanel.SetActive(false);
        StatPanel.SetActive(false);
        WarningPanel.SetActive(false);
        PausePanel.SetActive(false);
    }
    public void OnApplicationQuit()
    {
        SaveAll();
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        CloseAllPanel();
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMenuScene()
    {
        sceneTransition.SetActive(true);
        SaveAll();
        SceneTransition.SwitchToScene("Menu");
    }
}
