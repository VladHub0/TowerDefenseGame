using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameLifecycles : MonoBehaviour, IGameLifecycles
{
    [Header("DI registrations (assign in Inspector)")]
    [SerializeField] private GetMainFieldTransform provider;
    [SerializeField] private Camera cameraToRegister;

    [Header("Input")]
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private MouseInput mouseInputComponent;


    [Header("UI Panels (assign build/upgrade panels here)")]
    [SerializeField] private GameObject buildPanel; 
    [SerializeField] private GameObject upgradePanel; 
    [SerializeField] private GameObject rootCanvas;


    private DiContainer _container;
    private IFieldProvider fieldProvider;
    private void Init()
    {
        if(provider == null)
        {
            fieldProvider = null;
            Debug.Log("The Provaider is null");
            return;
        }

        if(provider.TryGetComponent<IFieldProvider>(out var field))
        {
            fieldProvider = field;
        }
        else
        {
            fieldProvider = provider as IFieldProvider;
        }
    }
    

    public void Initialize()
    {
        _container = new DiContainer();

        if (fieldProvider != null)
        {
            _container.Register<IFieldProvider>(fieldProvider);
        }
        else
        {
            Debug.LogWarning("GameLifecycles: fieldProvider not assigned in Inspector. CameraSetUp injection may fail.");
        }

        if (cameraToRegister != null)
        {
            _container.Register<Camera>(cameraToRegister);
        }
        else
        {
            Debug.LogWarning("GameLifecycles: cameraToRegister not assigned in Inspector. CameraSetUp injection may fail.");
        }


        if (actionAsset != null)
        {   
            var inputReader = new UnityInputReader(actionAsset);
            _container.Register<IInputReader>(inputReader); 
            var mapSwitcher = new SimpleMapSwitcher(actionAsset);
            _container.Register<IInputMapSwitcher>(mapSwitcher);
        }
        else
        { 
            Debug.LogWarning("GameLifecycles: actionAsset not assigned in Inspector.");
        } 


        Camera rayCamera = null;
        try 
        {
            rayCamera = _container.Resolve<Camera>();
        } 
        catch
        {
            rayCamera = Camera.main;
        } 

        if (rayCamera != null) 
        { 
            var raycaster = new CameraRaycaster(rayCamera); 
            _container.Register<IRaycaster>(raycaster);
        }
        else 
        {
            Debug.LogWarning("GameLifecycles: no Camera available for CameraRaycaster.");
        } 
        
        if (buildPanel != null && upgradePanel != null)
        { 
            var uiFlow = new UiFlowManager(buildPanel, upgradePanel, rootCanvas); 
            _container.Register<IUiFlow>(uiFlow);
        } 
        else
        { 
            Debug.LogWarning("GameLifecycles: buildPanel or upgradePanel not assigned. UI flow will not be available.");
        }
        if (mouseInputComponent != null) 
        { 
            try { 
                var inputReader = _container.Resolve<IInputReader>();
                var raycaster = _container.Resolve<IRaycaster>();
                var mapSwitcher = _container.Resolve<IInputMapSwitcher>();
                var uiFlow = _container.Resolve<IUiFlow>();
                mouseInputComponent.Init(inputReader, raycaster, mapSwitcher, uiFlow);
            }
            catch (KeyNotFoundException knf) 
            { 
                Debug.LogError($"GameLifecycles: missing dependency for MouseInput: {knf.Message}");
            }
            catch (System.Exception ex)
            { 
                Debug.LogError($"GameLifecycles: failed to init MouseInput: {ex.Message}");
            } }
        else
        { 
            Debug.LogWarning("GameLifecycles: mouseInputComponent not assigned in Inspector."); 
        }

        var allMono = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var mb in allMono)
        {
            _container.InjectTo(mb);
        }
    }

    private void Awake()
    {
        Init();
        Initialize();
    }
}

