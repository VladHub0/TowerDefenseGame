using System;
using UnityEngine;

public class TowerConstructionManager : MonoBehaviour,
    IBuildAction, IUpgradeAction, ICancelAction, IDemolishAction
{
    [Header("Dependencies (MonoBehaviour components implementing required interfaces)")]
    [SerializeField] private MonoBehaviour _mouseInputMono;
    [SerializeField] private MonoBehaviour _uiFlowMono;
    [SerializeField] private MonoBehaviour _mapSwitcherMono;
    [SerializeField] private MonoBehaviour _towerBuilderMono;


    [SerializeField] private MonoBehaviour _currencyServiceMono;


    [Header("Tower Prices")]
    [SerializeField] private int[] _towerPrices; 

    [Header("Upgrade Settings")]
    [SerializeField] private GameObject _upgradedTowerPrefab;

    private IMouseInputEvents _mouseInputEvents;
    private IUiFlow _uiFlow;
    private IInputMapSwitcher _mapSwitcher;
    private ITowerBuilder _towerBuilder;
    private ICurrencyService _currencyService;

    private GameObject _currentPlace;
    private GameObject _currentTower;


    public int[] GetTowerPrices() => _towerPrices;


    private void Awake()
    {
        _mouseInputEvents = _mouseInputMono as IMouseInputEvents;
        _uiFlow = _uiFlowMono as IUiFlow;
        _mapSwitcher = _mapSwitcherMono as IInputMapSwitcher;
        _towerBuilder = _towerBuilderMono as ITowerBuilder;

        _currencyService = _currencyServiceMono as ICurrencyService; 

        if (_mouseInputEvents == null)
        {
            Debug.LogError($"[{nameof(TowerConstructionManager)}] {_mouseInputMono?.name} does not implement {nameof(IMouseInputEvents)}!", this);
            return;
        }
        if (_uiFlow == null)
        {
            Debug.LogError($"[{nameof(TowerConstructionManager)}] {_uiFlowMono?.name} does not implement {nameof(IUiFlow)}!", this);
            return;
        }
        if (_mapSwitcher == null)
        {
            Debug.LogError($"[{nameof(TowerConstructionManager)}] {_mapSwitcherMono?.name} does not implement {nameof(IInputMapSwitcher)}!", this);
            return;
        }
        if (_towerBuilder == null)
        {
            Debug.LogError($"[{nameof(TowerConstructionManager)}] {_towerBuilderMono?.name} does not implement {nameof(ITowerBuilder)}!", this);
            return;
        }
        if (_currencyService == null)
            Debug.LogError($"[{nameof(TowerConstructionManager)}] {_currencyServiceMono?.name} does not implement {nameof(ICurrencyService)}!", this);

        _mouseInputEvents.OnBuildUIRequested += OnBuildUIRequested;
        _mouseInputEvents.OnTowerUIUpgrateRequested += OnTowerUIUpgradeRequested;
        _mouseInputEvents.OnClickEmptyPlace += OnClickEmptyPlace;
    }

    private void OnDestroy()
    {
        if (_mouseInputEvents != null)
        {
            _mouseInputEvents.OnBuildUIRequested -= OnBuildUIRequested;
            _mouseInputEvents.OnTowerUIUpgrateRequested -= OnTowerUIUpgradeRequested;
            _mouseInputEvents.OnClickEmptyPlace -= OnClickEmptyPlace;
        }
    }

    private void OnBuildUIRequested(object sender, BuildUITowerEventArgs args)
    {
        _currentPlace = args.PlaceObject;
        _currentTower = null;
    }

    private void OnTowerUIUpgradeRequested(object sender, TowerUpgrateUIArgs args)
    {
        _currentTower = args.TowerObject;
        _currentPlace = null;
    }

    private void OnClickEmptyPlace(object sender, EventArgs e)
    {
        _currentPlace = null;
        _currentTower = null;
    }

    public void ExecuteBuild(int towerIndex)
    {
        if (_currentPlace == null)
        {
            Debug.LogWarning("Не выбрано место для строительства!");
            return;
        }

        if (towerIndex < 0 || towerIndex >= _towerPrices.Length)
        {
            Debug.LogError("Некорректный индекс башни или не задана цена!");
            return;
        }

        int price = _towerPrices[towerIndex];
        if (!_currencyService.SpendCurrency(price))
        {
            Debug.Log("Недостаточно монет для постройки этой башни!");
            return;
        }

        GameObject newTower = _towerBuilder.BuildTower(_currentPlace, towerIndex);
        if (newTower == null)
        {
            Debug.LogError("Не удалось создать башню!");
            _currencyService.AddCurrency(price);
            return;
        }

        _currentPlace.SetActive(false);
        FinishInteraction();
    }

    public void ExecuteUpgrade()
    {
        if (_currentTower == null)
        {
            Debug.LogWarning("Не выбрана башня для улучшения!");
            return;
        }

        if (_upgradedTowerPrefab == null)
        {
            Debug.LogError("Не назначен префаб улучшенной башни!");
            return;
        }

        // Сохраняем ссылку на место перед уничтожением башни
        TowerData oldData = _currentTower.GetComponent<TowerData>();
        GameObject place = oldData != null ? oldData.PlaceObject : null;

        Vector3 position = _currentTower.transform.position;
        Quaternion rotation = _currentTower.transform.rotation;
        Destroy(_currentTower);

        // Создаём улучшенную башню
        GameObject newTower = Instantiate(_upgradedTowerPrefab, position, rotation);
        if (place != null)
        {
            var newData = newTower.AddComponent<TowerData>();
            newData.PlaceObject = place;
        }

        FinishInteraction();
    }

    public void ExecuteDemolish()
    {
        if (_currentTower == null)
        {
            Debug.LogWarning("Не выбрана башня для сноса!");
            return;
        }

        TowerData data = _currentTower.GetComponent<TowerData>();
        if (data != null && data.PlaceObject != null)
        {
            data.PlaceObject.SetActive(true);
        }

        if (data != null && data.TowerTypeIndex >= 0 && data.TowerTypeIndex < _towerPrices.Length)
        {
            int price = _towerPrices[data.TowerTypeIndex];
            int refund = Mathf.RoundToInt(price * 0.75f);
            _currencyService.AddCurrency(refund);
        }

        Destroy(_currentTower);
        FinishInteraction();
    }

    public void ExecuteCancel()
    {
        FinishInteraction();
    }

    private void FinishInteraction()
    {
        _currentPlace = null;
        _currentTower = null;
        _uiFlow.CloseAll();
        _mapSwitcher.SwitchTo("MouseInput");
    }
}