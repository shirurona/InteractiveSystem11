using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public void ShowScore(string showText)
    {
        scoreText.text = showText;
    }
}
