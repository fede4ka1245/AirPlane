using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class BoxMenu : MonoBehaviour
{
    int coins;
    int boxes;

    [SerializeField] Text coinsText;
    [SerializeField] Text boxesText;

    Save sv = new Save();

    [SerializeField] GameObject WarningPanel;
    [SerializeField] GameObject PanelWithPrizes;
    [SerializeField] Text PrizeText;

    [SerializeField] GameObject Coins50;
    [SerializeField] GameObject Coins100;
    [SerializeField] GameObject Coins250;
    [SerializeField] GameObject Coins500;
    [SerializeField] GameObject Coins1000;
    [SerializeField] GameObject UFO;
    [SerializeField] GameObject OldBoot;

    GameObject prize;
    string prizeName;

    [SerializeField] GameObject Box;

    [SerializeField] GameObject ButtonGetBox;
    [SerializeField] GameObject ButtonGoToMenu;
    [SerializeField] GameObject ButtonOpenBox;

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
            boxes = sv.Boxes;
        }
        Time.timeScale = 1;

        ShowMainInfo();
    }

    void SetActiveAllButton(bool flag)
    {
        ButtonGetBox.SetActive(flag);
        ButtonGoToMenu.SetActive(flag);
        ButtonOpenBox.SetActive(flag);
    }

    IEnumerator CloseAllButtonCoroutine()
    {
        SetActiveAllButton(false);

        yield return new WaitForSeconds(0.2f);

        SetActiveAllButton(true);
    }

    void SaveAll()
    {
        sv.Coins = coins;
        sv.Boxes = boxes;

        PlayerPrefs.SetString("save", JsonUtility.ToJson(sv));
    }

    private void ShowMainInfo()
    {
        coinsText.text = coins.ToString().PadLeft(5, '0');
        boxesText.text = boxes.ToString() + "x";
    }

    public void GoToMenuScene()
    {
        sceneTransition.SetActive(true);
        SaveAll();
        SceneTransition.SwitchToScene("Menu");
    }

    public void CloseAllPanels()
    {
        WarningPanel.SetActive(false);
        PanelWithPrizes.SetActive(false);
    }

    void GetPrize()
    {
        int chance = Random.Range(0, 201);
        if (chance <= 99)
        {
            prize = OldBoot;
            prizeName = "Old Boot";
        }
        else if (chance >= 100 && chance <= 150)
        {
            coins += 100;
            prize = Coins100;
            prizeName = "100 coins";
        }
        else if (chance > 150 && chance <= 190)
        {
            coins += 250;
            prize = Coins250;
            prizeName = "250 coins";
        }
        else if (chance > 190 && chance <= 195)
        {
            coins += 500;
            prize = Coins500;
            prizeName = "500 coins"; 
        }
        else if (chance > 195 && chance <= 199)
        {
            coins += 1000;
            prize = Coins1000;
            prizeName = "1000 coins";
        }
        else
        {
            if (PlayerPrefs.GetInt("UFO") == 0)
            {
                prize = UFO;
                prizeName = "UFO";
                PlayerPrefs.SetInt(prizeName, 1);
            }
            else
            {
                coins += 1000;
                prize = prize = Coins1000;
                prizeName = "1000 coins";
            }
        }
        SaveAll();
    }

    public void OpenBox()
    {
        if (boxes >= 1)
        {
            GetPrize();
            StartCoroutine(OpenBoxCoroutine());
            StartCoroutine(CloseAllButtonCoroutine());
            boxes -= 1;
            ShowMainInfo();
        }
        else
        {
            WarningPanel.SetActive(true);
        }
    }

    IEnumerator OpenBoxCoroutine()
    {
        Box.GetComponent<Animator>().SetBool("IsOpen", true);

        yield return new WaitForSeconds(0.2f);

        Box.GetComponent<Animator>().SetBool("IsOpen", false);
        PanelWithPrizes.SetActive(true);
        prize.SetActive(true);
        PrizeText.text = $"You get {prizeName}!";
    }

    public void ClosePrizePanel()
    {
        prize.SetActive(false);
        CloseAllPanels();
    }

    public void GetBox()
    {
        if (Advertisement.IsReady())
        {
            StartCoroutine(CloseButtonGetBoxCorutine());
            Advertisement.Show("Interstitial_iOS");
            boxes += 1;
            ShowMainInfo();
        }
        else
        {
            WarningPanel.SetActive(true);
        }
    }

    IEnumerator CloseButtonGetBoxCorutine()
    {
        ButtonGetBox.SetActive(false);

        yield return new WaitForSeconds(1f);

        ButtonGetBox.SetActive(true);
    }
}
