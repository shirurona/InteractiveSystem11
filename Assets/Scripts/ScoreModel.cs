using R3;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Score")]
public class ScoreModel : ScriptableObject
{
    public ReadOnlyReactiveProperty<int> ScorePoint => _score;
    [SerializeField] private SerializableReactiveProperty<int> _score = new SerializableReactiveProperty<int>(0);

    public void Reset()
    {
        _score.Value = 0;
    }
    
    public void OnCut()
    {
        _score.Value++;
    }
}
