using R3;
using UnityEngine;

public class ScorePresenter : MonoBehaviour
{
    [SerializeField] private ScoreModel model;
    [SerializeField] private ScoreView view;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        model.ScorePoint
            .Subscribe(ShowScore)
            .AddTo(this);
    }

    void ShowScore(int scorePoint)
    {
        string showText = $"SCORE:{scorePoint}";
        view.ShowScore(showText);
    }
}
