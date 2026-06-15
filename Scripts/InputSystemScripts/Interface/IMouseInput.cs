public interface IMouseInput { 
    void Init(IInputReader inputReader, IRaycaster raycaster, IInputMapSwitcher mapSwitcher, IUiFlow uiFlow);
}