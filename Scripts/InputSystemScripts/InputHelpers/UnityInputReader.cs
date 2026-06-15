using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnityInputReader : IInputReader
{
    private readonly InputActionAsset _asset;
    public UnityInputReader(InputActionAsset asset) => _asset = asset;

    public void Subscribe(string mapName, string actionName, Action<InputAction.CallbackContext> handler)
    {
        var map = _asset.FindActionMap(mapName, true);
        var action = map.FindAction(actionName, true);
        action.performed += handler;
        map.Enable();
    }

    public void Unsubscribe(string mapName, string actionName, Action<InputAction.CallbackContext> handler)
    {
        var map = _asset.FindActionMap(mapName, false);
        if (map == null) return;
        var action = map.FindAction(actionName, false);
        if (action == null) return;
        action.performed -= handler;
    }

    public Vector2 ReadPointer(string mapName, string pointerActionName)
    {
        var map = _asset.FindActionMap(mapName, true);
        var action = map.FindAction(pointerActionName, true);
        return action.ReadValue<Vector2>();
    }
}
