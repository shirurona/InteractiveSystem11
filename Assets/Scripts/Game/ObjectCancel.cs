using System.Threading;
using UnityEngine;

public class ObjectCancel : MonoBehaviour
{
    private CancellationTokenSource _cts;

    public void TryCancel()
    {
        _cts?.Cancel();
    }

    public CancellationToken GetToken()
    {
        _cts = new CancellationTokenSource();
        return _cts.Token;
    }
}
