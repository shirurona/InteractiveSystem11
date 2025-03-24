using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class RecordView : MonoBehaviour
{
    [SerializeField] private Image backgroundImage; 
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Animator animator;
    
    private static readonly string[] RankStr = { "1st", "2nd", "3rd", "4th", "5th" };
    private static readonly Color32[] Colors =
    {
        new Color32(251, 179, 72, 255),
        new Color32(127, 127, 127, 255),
        new Color32(234, 120, 60, 255),
        new Color32(46, 121, 56, 255),
        new Color32(46, 121, 56, 255)
    };

    private static readonly int HighlightAnimationHash = Animator.StringToHash("Highlight"); 
    
    public void SetRecord(int rankIndex, Record record)
    {
        rankText.color = Colors[rankIndex];
        rankText.text = RankStr[rankIndex];
        if (record.Name is null)
        {
            nameText.text = string.Empty;
            animator.Play(HighlightAnimationHash);
            Debug.Log("play");
        }
        else
        {
            nameText.text = record.Name;
        }
        scoreText.text = $"{record.Score}<size=23.6>コ</size>";
    }
}
