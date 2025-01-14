using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverResult : MonoBehaviour
{
    [SerializeField] private ScoreModel scoreModel;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    void Start()
    {
        AudioManager.Instance.PlaySE("end");
        scoreText.text = $"スコア：{scoreModel.ScorePoint.CurrentValue}<size=24>コ";
    }
    
    public void Retry()
    {
        AudioManager.Instance.PlaySE("decide");
        SceneManager.LoadScene("Demo");
    }

    public void Title()
    {
        AudioManager.Instance.PlaySE("decide");
        SceneManager.LoadScene("Title");
    }
}
