
public class CurrencyService : ICurrencyService
{
    private int _currentCurrency;
    private readonly IEventBus _eventBus;

    public int CurrentCurrency => _currentCurrency;

    public CurrencyService(IEventBus eventBus, int startCurrency)
    {
        _eventBus = eventBus;
        _currentCurrency = startCurrency;
        _eventBus.Invoke(new CurrencyChangedSignal(_currentCurrency));
    }

    public void AddCurrency(int amount)
    {
        _currentCurrency += amount;
        _eventBus.Invoke(new CurrencyChangedSignal(_currentCurrency));
    }

    public bool SpendCurrency(int amount)
    {
        if (_currentCurrency >= amount)
        {
            _currentCurrency -= amount;
            _eventBus.Invoke(new CurrencyChangedSignal(_currentCurrency));
            return true;
        }
        return false;
    }
}