using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class MouseInput : MonoBehaviour, IMouseInput, IMouseInputEvents
{
    [SerializeField] private string placeTag = "TowerPlace";
    [SerializeField] private string towerTag = "Tower";
    [SerializeField] private string groundTag = "MainField";

    private IInputReader _inputReader;
    private IRaycaster _raycaster;
    private IInputMapSwitcher _mapSwitcher;
    private IUiFlow _uiFlow;

    private const string MapName = "MouseInput";
    private const string ClickActionName = "MouseLC";
    private const string PointerActionName = "PointerPosition";

    public event EventHandler<BuildUITowerEventArgs> OnBuildUIRequested;
    public event EventHandler<TowerUpgrateUIArgs> OnTowerUIUpgrateRequested;
    public event EventHandler OnClickEmptyPlace;


    private static readonly List<RaycastResult> s_RaycastResults = new List<RaycastResult>();

    public void Init(IInputReader inputReader, IRaycaster raycaster, IInputMapSwitcher mapSwitcher, IUiFlow uiFlow)
    {
        _inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));
        _raycaster = raycaster ?? throw new ArgumentNullException(nameof(raycaster));
        _mapSwitcher = mapSwitcher ?? throw new ArgumentNullException(nameof(mapSwitcher));
        _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));

        _inputReader.Subscribe(MapName, ClickActionName, OnClick);
    }

    private void OnDestroy()
    {
        if (_inputReader != null)
            _inputReader.Unsubscribe(MapName, ClickActionName, OnClick);
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
       

        var screenPos = _inputReader.ReadPointer(MapName, PointerActionName);

        if (IsPointerOverUI(screenPos)) return;

        if (_raycaster.RaycastFromScreen(screenPos, out var hit, 1000f, ~LayerMask.GetMask("Trigger")))
        {
            HandleHit(hit.collider.gameObject);
        }
    }

    private void HandleHit(GameObject go)
    {
        if (go.CompareTag(placeTag))
        {
            OnBuildUIRequested?.Invoke(this, new BuildUITowerEventArgs(go));
            _uiFlow.OpenBuildMenu(go);
            _mapSwitcher.SwitchTo("UI");
        }
        else if (go.CompareTag(towerTag))
        {
            OnTowerUIUpgrateRequested?.Invoke(this, new TowerUpgrateUIArgs(go));
            _uiFlow.OpenTowerUpgrade(go);
            _mapSwitcher.SwitchTo("UI");
        }
        else if (go.CompareTag(groundTag))
        {
            OnClickEmptyPlace?.Invoke(this, EventArgs.Empty);
            _uiFlow.CloseAll();
            _mapSwitcher.SwitchTo("MouseInput");
        }
    }


    private bool IsPointerOverUI(Vector2 screenPos)
    {
        var es = EventSystem.current;
        if (es == null) return false; 
        var data = new PointerEventData(es)  { position = screenPos };
        s_RaycastResults.Clear(); 
        es.RaycastAll(data, s_RaycastResults);
        return s_RaycastResults.Count > 0;
    }
}
