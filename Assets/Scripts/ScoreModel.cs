using R3;
using UnityEngine;

public class ScoreModel : MonoBehaviour
{
    public ReadOnlyReactiveProperty<int> ScorePoint => _score;

    private ReactiveProperty<int> _score = new ReactiveProperty<int>(0);
    
    public void OnCut()
    {
        _score.Value++;
    }
}
