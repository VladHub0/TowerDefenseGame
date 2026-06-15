using CustomEventBus;
using CustomEventBus.Signals;
using TMPro;
using UnityEngine;

public class CastleHealth : MonoBehaviour, IDisposable
{
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private TextMeshProUGUI _healthText; 

    private int _currentHealth;
    private IEventBus _eventBus;

    public void Init(IEventBus eventBus)
    {
        _eventBus = eventBus;
        _eventBus.Subscribe<EnemyReachedCastleSignal>(OnEnemyReachedCastle);
        _currentHealth = _maxHealth;
        UpdateUI();
    }

    private void OnEnemyReachedCastle(EnemyReachedCastleSignal signal)
    {
        _currentHealth -= signal.DamageAmount;
        if (_currentHealth < 0) _currentHealth = 0;

        UpdateUI();
        Debug.Log($"Castle took {signal.DamageAmount} damage. HP = {_currentHealth}");

        if (_currentHealth <= 0)
        {
           
            Debug.Log("Game Over!");
            // SceneManager.LoadScene("LoseScene");
        }
    }

    private void UpdateUI()
    {
        if (_healthText != null)
            _healthText.text = _currentHealth.ToString();
    }

    public void Dispose()
    {
        if (_eventBus != null)
            _eventBus.Unsubscribe<EnemyReachedCastleSignal>(OnEnemyReachedCastle);
    }

    private void OnDestroy() => Dispose();
}