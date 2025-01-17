using R3;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Score")]
public class ScoreModel : ScriptableObject
{
    public ReadOnlyReactiveProperty<int> ScorePoint => _score;

    private ReactiveProperty<int> _score = new ReactiveProperty<int>(0);

    public void Reset()
    {
        _score.Value = 0;
    }
    
    public void OnCut()
    {
        _score.Value++;
    }
}
