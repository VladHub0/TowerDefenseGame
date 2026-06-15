using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleMapSwitcherWrapper : MonoBehaviour, IInputMapSwitcher
{
    [SerializeField] private InputActionAsset _inputActionAsset;

    private SimpleMapSwitcher _inner;

    private void Awake()
    {
        if (_inputActionAsset == null)
        {
            Debug.LogError($"[{nameof(SimpleMapSwitcherWrapper)}] InputActionAsset is not assigned!", this);
            return;
        }

        _inner = new SimpleMapSwitcher(_inputActionAsset);
    }

    public void SwitchTo(string mapName)
    {
        if (_inner == null)
        {
            Debug.LogError($"[{nameof(SimpleMapSwitcherWrapper)}] Inner SimpleMapSwitcher is not initialized!", this);
            return;
        }

        _inner.SwitchTo(mapName);
    }
}