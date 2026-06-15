using System;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputReader { 
    void Subscribe(string mapName, string actionName, Action<InputAction.CallbackContext> handler);
    void Unsubscribe(string mapName, string actionName, Action<InputAction.CallbackContext> handler);
    Vector2 ReadPointer(string mapName, string pointerActionName);
}