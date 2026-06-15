public interface ICurrencyService
{
    int CurrentCurrency { get; }
    void AddCurrency(int amount);
    bool SpendCurrency(int amount);
}