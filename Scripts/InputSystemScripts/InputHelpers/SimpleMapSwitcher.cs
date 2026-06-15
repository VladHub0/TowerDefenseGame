using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleMapSwitcher : IInputMapSwitcher
{
    private readonly InputActionAsset _asset;
    public SimpleMapSwitcher(InputActionAsset asset) => _asset = asset;

    public void SwitchTo(string mapName)
    {
        foreach (var map in _asset.actionMaps) map.Disable();
        _asset.FindActionMap(mapName, true).Enable();
    }
}
