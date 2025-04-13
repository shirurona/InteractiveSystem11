using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Leap.PhysicalHands;
using R3;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalHandsButtonCustom : PhysicalHandsButton
{
    [SerializeField] [Range(0f, 3f)] private float buttonLongPressTime = 1f; 
    [SerializeField] public UnityEvent OnButtonLongPress;
    public ReadOnlyReactiveProperty<float> LongPressProperty => _longPressProperty;
    private ReactiveProperty<float> _longPressProperty = new ReactiveProperty<float>(0);
    private ReactiveProperty<bool> _isLongPress = new ReactiveProperty<bool>(false);
    private CancellationTokenSource _longPressCancellationTokenSource;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _isLongPress
            .Subscribe(x =>
            {
                if (x)
                {
                    _longPressCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy(), new CancellationTokenSource().Token);
                    UniTask.Create(async ct =>
                    {
                        await UniTask.WaitForSeconds(buttonLongPressTime, cancellationToken: ct);
                        OnButtonLongPress?.Invoke();
                    }, _longPressCancellationTokenSource.Token);
                }
                else
                {
                    _longPressCancellationTokenSource?.Cancel();
                    _longPressProperty.Value = 0;
                }
            })
            .AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isLongPress.CurrentValue)
        {
            _longPressProperty.Value = Math.Min(_longPressProperty.CurrentValue + Time.deltaTime / buttonLongPressTime, 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _isLongPress.Value = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _isLongPress.Value = false;
    }
}
