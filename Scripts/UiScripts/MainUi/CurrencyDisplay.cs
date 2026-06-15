using TMPro;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour, IDisposable
{
    [SerializeField] private TextMeshProUGUI _currencyText;

    private IEventBus _eventBus;

    public void Init(IEventBus eventBus)
    {
        _eventBus = eventBus;
        _eventBus.Subscribe<CurrencyChangedSignal>(OnCurrencyChanged);
    }


    public void SetCurrency(int amount)
    {
        if (_currencyText != null)
            _currencyText.text = amount.ToString();
    }
    private void OnCurrencyChanged(CurrencyChangedSignal signal)
    {
        if (_currencyText != null)
            _currencyText.text = signal.NewAmount.ToString();
    }

    public void Dispose()
    {
        if (_eventBus != null)
            _eventBus.Unsubscribe<CurrencyChangedSignal>(OnCurrencyChanged);
    }

    private void OnDestroy()
    {
        Dispose();
    }
}