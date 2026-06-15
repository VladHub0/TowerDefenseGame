using UnityEngine;
using UnityEngine.InputSystem;

public class UnityInputReaderWrapper : MonoBehaviour, IInputReader
{
    [SerializeField] private InputActionAsset _inputActionAsset;

    private UnityInputReader _inner;

    private void Awake()
    {
        if (_inputActionAsset == null)
        {
            Debug.LogError($"[{nameof(UnityInputReaderWrapper)}] InputActionAsset is not assigned!", this);
            return;
        }

        _inner = new UnityInputReader(_inputActionAsset);
    }

    public void Subscribe(string mapName, string actionName, System.Action<InputAction.CallbackContext> handler)
    {
        if (_inner == null)
        {
            Debug.LogError($"[{nameof(UnityInputReaderWrapper)}] Inner UnityInputReader is not initialized!", this);
            return;
        }

        _inner.Subscribe(mapName, actionName, handler);
    }

    public void Unsubscribe(string mapName, string actionName, System.Action<InputAction.CallbackContext> handler)
    {
        if (_inner == null)
        {
            Debug.LogError($"[{nameof(UnityInputReaderWrapper)}] Inner UnityInputReader is not initialized!", this);
            return;
        }

        _inner.Unsubscribe(mapName, actionName, handler);
    }

    public Vector2 ReadPointer(string mapName, string pointerActionName)
    {
        if (_inner == null)
        {
            Debug.LogError($"[{nameof(UnityInputReaderWrapper)}] Inner UnityInputReader is not initialized!", this);
            return Vector2.zero;
        }

        return _inner.ReadPointer(mapName, pointerActionName);
    }
}