using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private ScoreModel scoreModel;
    [SerializeField] private RectTransform scoreTransform;
    [SerializeField] private GameObject[] disableObjects;
    [SerializeField] private Animator animator;
    private readonly int _gameOverHash = Animator.StringToHash("GameOver");
    public async UniTaskVoid OnGameOver(CancellationToken cts)
    {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySE("end");
        animator.Play(_gameOverHash);
        await UniTask.WaitForSeconds(0.1f, cancellationToken:cts);
        foreach (var disableObject in disableObjects)
        {
            disableObject.SetActive(false);
        }
        scoreTransform.anchoredPosition = new Vector2(0, 100);
        scoreText.text = $"スコア：{scoreModel.ScorePoint.CurrentValue}<size=24>コ";
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        AudioManager.Instance.PlaySE("decide");
        SceneManager.LoadScene("Demo");
        Time.timeScale = 1;
    }

    public void Title()
    {
        AudioManager.Instance.PlaySE("decide");
        SceneManager.LoadScene("Title");
        Time.timeScale = 1;
    }
}
