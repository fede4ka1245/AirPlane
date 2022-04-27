using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] int coins;
    int bestScore;
    [SerializeField] int boxes;
    [SerializeField] Text coinsText;
    [SerializeField] Text bestScoreText;
    [SerializeField] Text boxesText;
    [SerializeField] Text PriceText;

    Save sv = new Save();

    [SerializeField] MenuPlane[] Planes;
    int planesIndex;
    MenuPlane plane;

    [SerializeField] GameObject ButtonPlay;
    [SerializeField] GameObject ButtonBuy;

    [SerializeField] GameObject WarningPanel;
    [SerializeField] GameObject BuyPanel;

    [SerializeField] GameObject AboutPanel;
    [SerializeField] Text AboutPanelText;

    [SerializeField] GameObject sceneTransition;

    private void Start()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            sv = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("save"));
            coins = sv.Coins;
            bestScore = sv.BestScore;
            boxes = sv.Boxes;
            planesIndex = 0;
        }
        else
        {
            planesIndex = 0;
            coins = 500;
            boxes = 3;
            bestScore = 0;
        }

        if (sv.BestScore >= 15000)
            PlayerPrefs.SetInt("Copter", 1);

        ShowMainInfo();
        ShowPlane();
    }

    private void ShowMainInfo()
    {
        coinsText.text = coins.ToString().PadLeft(5, '0');
        bestScoreText.text = "HI " + bestScore.ToString().PadLeft(5, '0');
        boxesText.text = boxes.ToString() + "x";
    }

    public void SaveAll()
    {
        sv.Coins = coins;
        sv.BestScore = bestScore;
        sv.Boxes = boxes;
        sv.PlanesIndex = planesIndex;
        sv.PlanesName = plane.Name;

        PlayerPrefs.SetString("save", JsonUtility.ToJson(sv));
    }

    private void ShowPlane()
    {
        plane = Planes[planesIndex];
        plane.gameObject.SetActive(true);
        if (plane.IsOpend == 1 || PlayerPrefs.GetInt(plane.Name) == 1)
        {
            AboutPanel.SetActive(false);
            ButtonBuy.SetActive(false);
            ButtonPlay.SetActive(true);
        }
        else if (plane.Name == "UFO" || plane.Name == "Copter")
        {
            AboutPanel.SetActive(true);
            ButtonBuy.SetActive(false);
            ButtonPlay.SetActive(false);

            AboutPanelText.text = plane.About;
        }
        else
        {
            AboutPanel.SetActive(false);
            ButtonBuy.SetActive(true);
            ButtonPlay.SetActive(false);

            PriceText.text = plane.Price.ToString();
        }
    }

    public void ShowNextPlane()
    {
        plane.gameObject.SetActive(false);
        if (planesIndex == Planes.Length - 1)
            planesIndex = 0;
        else
            planesIndex++;
        ShowPlane();
    }

    public void ShowPrevPlane()
    {
        plane.gameObject.SetActive(false);
        if (planesIndex == 0)
            planesIndex = Planes.Length - 1;
        else
            planesIndex--;
        ShowPlane();
    }

    public void Buy()
    {
        if (plane.Price <= coins)
        {
            plane.IsOpend = 1;
            PlayerPrefs.SetInt(plane.Name, 1);
            coins -= plane.Price;
            ShowPlane();
        }
        ShowMainInfo();
        CloseAllPanels();
    }

    public void OpenBuyPanel()
    {
        if (plane.Price <= coins)
        {
            BuyPanel.SetActive(true);
        }
        else
        {
            WarningPanel.SetActive(true);
        }
    }

    public void CloseAllPanels()
    {
        WarningPanel.SetActive(false);
        BuyPanel.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        SaveAll();
    }

    public void GoToGameScene()
    {
        sceneTransition.SetActive(true);
        SaveAll();
        SceneTransition.SwitchToScene("Game");
    }    

    public void GoToBoxesScene()
    {
        sceneTransition.SetActive(true);
        SaveAll();
        SceneTransition.SwitchToScene("Boxes");
    }
}
