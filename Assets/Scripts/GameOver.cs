using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private RectTransform scoreTransform;
    [SerializeField] private GameObject[] disableObjects;
    public async UniTaskVoid OnGameOver()
    {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySE("end");
        await UniTask.WaitForSeconds(0.1f);
        foreach (var disableObject in disableObjects)
        {
            disableObject.SetActive(false);
        }
        scoreTransform.anchoredPosition = new Vector2(0, 100);
        gameOverText.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
}
