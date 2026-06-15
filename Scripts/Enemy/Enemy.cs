using CustomEventBus.Signals;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _data;

    private CharacterController _controller;
    private IMover _mover;
    private IEnemyState _currentState;
    private IPath _path;
    private IAvoidanceProvider _avoidanceProvider;

    private IEventBus _eventBus;

    private int _currentHealth;
    private float _currentSpeed;
    private float _currentTurnSpeed;
    private bool _isAlive = true;

    public IPath Path => _path;
    public float Speed => _currentSpeed;
    public float TurnSpeed => _currentTurnSpeed;
    public IMover Mover => _mover;
    public bool IsAlive => _isAlive;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        if (_controller == null)
            _controller = gameObject.AddComponent<CharacterController>();
        _mover = new CharacterControllerMover(_controller);
        _avoidanceProvider = new DefaultAvoidanceProvider(); 
    }

    public void Initialize(IPath path, IEventBus eventBus)
    {
        if (_data == null)
        {
            Debug.LogError($"EnemyData not assigned on {name}", this);
            return;
        }

        _path = path;
        _path.Reset();

        _eventBus = eventBus;

        _currentHealth = _data.maxHealth;
        _currentSpeed = _data.speed;
        _currentTurnSpeed = _data.turnSpeed;
        _isAlive = true;
        SetState(new MoveState());
    }

    public void SetState(IEnemyState newState)
    {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState?.Enter(this);
    }

    private void Update()
    {
        if (!_isAlive) return;
        _currentState?.Update(this);
    }

    public Vector3 GetAvoidanceVector(Vector3 forwardDir)
    {
        return _avoidanceProvider.CalculateAvoidance(
            transform.position,
            forwardDir,
            _data.avoidRadius,
            _data.avoidStrength,
            _data.avoidWeight,
            _data.avoidAngle,
            LayerMask.GetMask("Enemy")
        );
    }
    public void TakeDamage(int damage, bool isPhysical = true)
    {
        if (!_isAlive) return;

        int finalDamage = damage;
        if (isPhysical && _data.physicalDamageReduction > 0f)
            finalDamage = Mathf.RoundToInt(damage * (1f - _data.physicalDamageReduction));

        _currentHealth -= finalDamage;
        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        _isAlive = false;
        Destroy(gameObject, 0.1f);
    }

    public void ReachCastle()
    {
        if (!_isAlive) return;
        _eventBus.Invoke(new EnemyReachedCastleSignal(_data.damageToCastle));
        Destroy(gameObject);
    }
}