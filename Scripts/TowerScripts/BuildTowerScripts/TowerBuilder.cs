using UnityEngine;

public class TowerBuilder : MonoBehaviour, ITowerBuilder
{
    [SerializeField] private GameObject[] _towerPrefabs;
    [SerializeField] private MonoBehaviour _positionProviderMono;

    private IPositionProvider _positionProvider;

    private void Awake()
    {
        _positionProvider = _positionProviderMono as IPositionProvider;
        if (_positionProvider == null)
            Debug.LogError($"[{nameof(TowerBuilder)}] Position provider missing!", this);
    }

    public GameObject BuildTower(GameObject place, int towerIndex)
    {
        if (towerIndex < 0 || towerIndex >= _towerPrefabs.Length)
        {
            Debug.LogError($"Invalid tower index {towerIndex}");
            return null;
        }

        GameObject prefab = _towerPrefabs[towerIndex];
        Vector3 position = _positionProvider != null
            ? _positionProvider.GetPlacementPosition(place, prefab)
            : place.transform.position;

        GameObject tower = Instantiate(prefab, position, Quaternion.identity);
        var data = tower.AddComponent<TowerData>();
        data.PlaceObject = place;
        data.TowerTypeIndex = towerIndex;
        return tower;
    }
}