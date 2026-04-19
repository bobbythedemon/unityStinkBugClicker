using UnityEngine;
using TMPro;
using System;

public class IdleGame : MonoBehaviour
{
    [Header("Currency")]
    public double points = 0;

    [Header("Progression")]
    public int pointsPerClick = 1;
    public int autoIncome = 0;

    [Header("Upgrade")]
    public int upgradeCost = 500;
    public bool upgradeBought = false;

    [Header("UI")]
    public TextMeshProUGUI pointsText;

    private string lastSaveKey = "lastSaveTime";

    void Start()
    {
        LoadGame();

        // hide upgrade button if already bought
        if (upgradeBought)
        {
            GameObject obj = GameObject.Find("UpgradeButton");
            if (obj != null) obj.SetActive(false);
        }

        InvokeRepeating(nameof(AutoGain), 1f, 1f);
        InvokeRepeating(nameof(AutoSave), 1f, 1f);

        UpdateUI();
    }

    // 🖱️ CLICK
    public void Click()
    {
        points += pointsPerClick;
        UpdateUI();
    }

    // 🤖 IDLE INCOME
    void AutoGain()
    {
        points += autoIncome;
        UpdateUI();
    }

    // 💰 UPGRADE (single purchase)
    public void BuyUpgrade()
    {
        if (upgradeBought) return;

        if (points >= upgradeCost)
        {
            points -= upgradeCost;

            pointsPerClick *= 2;
            autoIncome += 1;

            upgradeBought = true;

            GameObject obj = GameObject.Find("UpgradeButton");
            if (obj != null) obj.SetActive(false);

            SaveGame();
            UpdateUI();
        }
    }

    // 💾 SAVE
    void SaveGame()
    {
        PlayerPrefs.SetString("points", points.ToString());
        PlayerPrefs.SetInt("click", pointsPerClick);
        PlayerPrefs.SetInt("auto", autoIncome);
        PlayerPrefs.SetInt("upg", upgradeBought ? 1 : 0);

        PlayerPrefs.SetString(lastSaveKey, DateTime.Now.ToBinary().ToString());

        PlayerPrefs.Save();
    }

    // 📂 LOAD + OFFLINE PROGRESS
    void LoadGame()
    {
        if (PlayerPrefs.HasKey("points"))
            points = double.Parse(PlayerPrefs.GetString("points"));

        pointsPerClick = PlayerPrefs.GetInt("click", 1);
        autoIncome = PlayerPrefs.GetInt("auto", 0);
        upgradeBought = PlayerPrefs.GetInt("upg", 0) == 1;

        // offline earnings
        if (PlayerPrefs.HasKey(lastSaveKey))
        {
            long lastTime = Convert.ToInt64(PlayerPrefs.GetString(lastSaveKey));
            DateTime lastDate = DateTime.FromBinary(lastTime);

            TimeSpan diff = DateTime.Now - lastDate;

            int secondsAway = (int)diff.TotalSeconds;

            points += secondsAway * autoIncome;
        }
    }

    // 💾 autosave every second
    void AutoSave()
    {
        SaveGame();
    }

    void UpdateUI()
    {
        if (pointsText != null)
            pointsText.text = "Points: " + Math.Floor(points);
    }
}