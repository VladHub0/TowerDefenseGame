using TMPro;
using UnityEngine;

public class TowerPriceDisplay : MonoBehaviour
{
    [SerializeField] private TowerConstructionManager _towerManager;
    [SerializeField] private TextMeshProUGUI[] _priceTexts; 

    private void Start()
    {
        if (_towerManager == null)
        {
            Debug.LogError("TowerManager not assigned in TowerPriceDisplay!", this);
            return;
        }

        int[] prices = _towerManager.GetTowerPrices();
        for (int i = 0; i < prices.Length && i < _priceTexts.Length; i++)
        {
            if (_priceTexts[i] != null)
                _priceTexts[i].text = prices[i].ToString();
        }
    }
}