using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private EnemyData[] _enemyTypes;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnInterval = 2f;

    [Header("Dependencies")]
    [SerializeField] private PathManager _pathManager;
    [SerializeField] private MonoBehaviour _positionProviderMono; 
    [SerializeField] private EventBusComponent _eventBusComponent;

    private IEventBus _eventBus;
    private IPositionProvider _positionProvider;

    private void Awake()
    {
        _eventBus = _eventBusComponent;
        _positionProvider = _positionProviderMono as IPositionProvider;
        if (_positionProvider == null)
            Debug.LogError("PositionProvider not assigned or does not implement IPositionProvider", this);
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (_enemyTypes.Length == 0 || _spawnPoints.Length == 0) return;

        int spawnIndex = Random.Range(0, _spawnPoints.Length);
        Transform spawnPoint = _spawnPoints[spawnIndex];

        IPath path = _pathManager.GetPathForSpawn(spawnPoint.gameObject);
        if (path == null)
        {
            Debug.LogWarning($"No path for spawn point {spawnPoint.name}", this);
            return;
        }

        EnemyData chosen = _enemyTypes[Random.Range(0, _enemyTypes.Length)];
        if (chosen.prefab == null)
        {
            Debug.LogError($"EnemyData {chosen.name} has no prefab!", this);
            return;
        }

        Vector3 spawnPosition = _positionProvider != null
            ? _positionProvider.GetPlacementPosition(spawnPoint.gameObject, chosen.prefab)
            : spawnPoint.position;

        GameObject enemyObj = Instantiate(chosen.prefab, spawnPosition, spawnPoint.rotation);
        if (enemyObj.TryGetComponent<Enemy>(out var enemy))
            enemy.Initialize(path, _eventBus);
        else
            Debug.LogError($"Prefab {chosen.prefab.name} missing Enemy component!", this);
    }
}