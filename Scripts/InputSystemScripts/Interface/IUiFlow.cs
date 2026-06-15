using System;
using UnityEngine;

public interface IUiFlow
{
    void OpenBuildMenu(GameObject place);
    void OpenTowerUpgrade(GameObject tower);
    void CloseAll();
    bool IsAnyOpen { get; }
    event EventHandler OnOpened;
    event EventHandler OnClosed;
}
