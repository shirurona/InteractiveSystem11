using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingView : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform scrollView;
    [SerializeField] private RecordView recordPrefab;
    [SerializeField] private GameObject nameRegisterCanvas;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button nameDecideButton;

    private List<RecordView> _recordViews = new List<RecordView>();
    private UniTaskCompletionSource<string> _tcs = new UniTaskCompletionSource<string>();
    
    private void DecideName()
    {
        if (_tcs.Task.Status != UniTaskStatus.Succeeded)
        {
            _tcs.TrySetResult(nameInputField.text);
        }
    }

    public async UniTask<string> NameRegisterAsync()
    {
        nameDecideButton.onClick.AddListener(DecideName);
        nameRegisterCanvas.SetActive(true);
        string decideName = await _tcs.Task;
        nameRegisterCanvas.SetActive(false);
        return decideName;
    }

    public void ShowRanking(Record[] records)
    {
        Debug.Log("show ranking");
        ClearRanking();
        for (int i = 0; i < records.Length; i++)
        {
            RecordView recordView = Instantiate(recordPrefab, content);
            _recordViews.Add(recordView);
            recordView.SetRecord(i, records[i]);
        }
    }

    private void ClearRanking()
    {
        foreach (RecordView recordView in _recordViews)
        {
            Destroy(recordView.gameObject);
        }
        _recordViews.Clear();
    } 

    public void SetInputFieldPosition(int rank)
    {
        nameInputField.GetComponent<RectTransform>().anchoredPosition = new Vector2(-22.5f, scrollView.anchoredPosition.y + scrollView.sizeDelta.y / 2 - recordPrefab.GetComponent<RectTransform>().sizeDelta.y * (rank + 0.5f));
    }
}
