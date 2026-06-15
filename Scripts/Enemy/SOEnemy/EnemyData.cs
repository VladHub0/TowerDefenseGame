using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Info")]
    public EnemyType enemyType;
    public string enemyName;
    public GameObject prefab;          

    [Header("Stats")]
    public int maxHealth = 35;
    public float speed = 1f;
    public int damageToCastle = 6;
    public float turnSpeed = 5f;     

    [Header("Armor")]
    [Range(0f, 1f)]
    public float physicalDamageReduction = 0f;


    [Header("Avoidance")]
    public float avoidRadius = 1.5f;           // радиус обнаружения
    public float avoidStrength = 1f;           // базовая сила отталкивания
    public float avoidWeight = 0.5f;           // вес уклонения (0-1), уменьшает влияние
    [Range(0f, 180f)]
    public float avoidAngle = 120f;            // угол обзора для учёта врагов (по умолчанию 120°)   
}