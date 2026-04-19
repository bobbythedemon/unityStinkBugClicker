using UnityEngine;
using TMPro;

public class PointManager : MonoBehaviour
{
    public int points = 0;
    public TextMeshProUGUI pointsText;

    void Start()
    {
        UpdateUI();
    }

    public void AddPoint()
    {
        points += 1;
        UpdateUI();
    }

    void UpdateUI()
    {
        pointsText.text = "Points: " + points;
    }
}