using R3;
using UnityEngine;

public class ScorePresenter : MonoBehaviour
{
    [SerializeField] private ScoreModel model;
    [SerializeField] private ScoreView view;
    [SerializeField] private GameObjectPool objectCutModel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        model.Reset();
        
        objectCutModel.OnCut
            .Subscribe(_ => model.OnCut());
        
        model.ScorePoint
            .Subscribe(ShowScore)
            .AddTo(this);
    }

    void ShowScore(int scorePoint)
    {
        string showText = $"スコア:{scorePoint}";
        view.ShowScore(showText);
    }
}
