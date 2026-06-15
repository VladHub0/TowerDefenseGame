using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private CurrencyDisplay _currencyDisplay;
    [SerializeField] private EventBusComponent _eventBusComponent;
    [SerializeField] private CurrencyServiceWrapper _currencyService;
    [SerializeField] private CastleHealth _castleHealth;

    private void Awake()
    {
        IEventBus eventBus = _eventBusComponent;

       
        _currencyDisplay.Init(eventBus);

        _castleHealth.Init(eventBus);

    }

    private void Start()
    {
        _currencyDisplay.SetCurrency(_currencyService.CurrentCurrency);
    }
}