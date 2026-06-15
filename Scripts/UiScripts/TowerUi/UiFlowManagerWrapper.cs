using System;
using UnityEngine;

public class UiFlowManagerWrapper : MonoBehaviour, IUiFlow
{
    [SerializeField] private GameObject _buildPanel;
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private GameObject _rootCanvas;

    private UiFlowManager _inner; 

    public bool IsAnyOpen => _inner != null && _inner.IsAnyOpen;

    public event EventHandler OnOpened
    {
        add => _inner.OnOpened += value;
        remove => _inner.OnOpened -= value;
    }

    public event EventHandler OnClosed
    {
        add => _inner.OnClosed += value;
        remove => _inner.OnClosed -= value;
    }

    private void Awake()
    {
        if (_buildPanel == null)
        {
            Debug.LogError($"[{nameof(UiFlowManagerWrapper)}] Build panel is not assigned!", this);
            return;
        }
        if (_upgradePanel == null)
        {
            Debug.LogError($"[{nameof(UiFlowManagerWrapper)}] Upgrade panel is not assigned!", this);
            return;
        }

        _inner = new UiFlowManager(_buildPanel, _upgradePanel, _rootCanvas);
    }

    public void OpenBuildMenu(GameObject place)
    {
        if (_inner == null)
        {
            Debug.LogError($"[{nameof(UiFlowManagerWrapper)}] Inner UiFlowManager is not initialized!", this);
            return;
        }
        _inner.OpenBuildMenu(place);
    }

    public void OpenTowerUpgrade(GameObject tower)
    {
        if (_inner == null)
        {
            Debug.LogError($"[{nameof(UiFlowManagerWrapper)}] Inner UiFlowManager is not initialized!", this);
            return;
        }
        _inner.OpenTowerUpgrade(tower);
    }

    public void CloseAll()
    {
        if (_inner == null)
        {
            Debug.LogError($"[{nameof(UiFlowManagerWrapper)}] Inner UiFlowManager is not initialized!", this);
            return;
        }
        _inner.CloseAll();
    }
}