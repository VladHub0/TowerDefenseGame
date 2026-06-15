using UnityEngine;

public class CurrencyServiceWrapper : MonoBehaviour, ICurrencyService
{
    [SerializeField] private int _startCurrency = 300;
    [SerializeField] private EventBusComponent _eventBusComponent;

    private CurrencyService _inner;

    private void Awake()
    {
        IEventBus eventBus = _eventBusComponent;
        _inner = new CurrencyService(eventBus, _startCurrency);
    }

    public int CurrentCurrency => _inner.CurrentCurrency;
    public void AddCurrency(int amount) => _inner.AddCurrency(amount);
    public bool SpendCurrency(int amount) => _inner.SpendCurrency(amount);
}