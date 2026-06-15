using System;
using UnityEngine;

public class UiFlowManager :  IUiFlow
{
    private readonly GameObject _buildPanel;
    private readonly GameObject _upgradePanel;
    private readonly GameObject _rootCanvas; 

    public bool IsAnyOpen { get; private set; }

    public event EventHandler OnOpened;
    public event EventHandler OnClosed;

    public UiFlowManager(GameObject buildPanel, GameObject upgradePanel, GameObject rootCanvas = null)
    {
        _buildPanel = buildPanel ?? throw new ArgumentNullException(nameof(buildPanel));
        _upgradePanel = upgradePanel ?? throw new ArgumentNullException(nameof(upgradePanel));
        _rootCanvas = rootCanvas;
        CloseAll();
    }

    private void Awake()
    {
        CloseAll();
    }

    public void OpenBuildMenu(GameObject place)
    {
        if (place == null) throw new ArgumentNullException(nameof(place));
        CloseAllInternal();

        _buildPanel.SetActive(true);
        IsAnyOpen = true;
        OnOpened?.Invoke(this, EventArgs.Empty);
    }

    public void OpenTowerUpgrade(GameObject tower)
    {
        if (tower == null) throw new ArgumentNullException(nameof(tower));
        CloseAllInternal();

        _upgradePanel.SetActive(true);
        IsAnyOpen = true;
        OnOpened?.Invoke(this, EventArgs.Empty);
    }

    public void CloseAll()
    {
        CloseAllInternal();
        OnClosed?.Invoke(this, EventArgs.Empty);
    }

    private void CloseAllInternal()
    {
        _buildPanel.SetActive(false);
        _upgradePanel.SetActive(false);
        if (_rootCanvas != null)
            _rootCanvas.SetActive(true); 
        IsAnyOpen = false;
    }
}
