using UnityEngine;

public class MoveState : IEnemyState
{
    private Vector3 _currentTarget;

    public void Enter(Enemy enemy)
    {
        GameObject firstNode = enemy.Path?.GetCurrentNode();
        if (firstNode != null)
            _currentTarget = firstNode.transform.position;
    }

    public void Update(Enemy enemy)
    {
        if (enemy.Path == null) return;

        Vector3 toTarget = (_currentTarget - enemy.transform.position).normalized;
        Vector3 avoidance = enemy.GetAvoidanceVector(toTarget);
        Vector3 desiredDir = (toTarget + avoidance).normalized;

        enemy.Mover.MoveInDirection(enemy.transform, desiredDir, enemy.Speed, enemy.TurnSpeed);

        if (enemy.Mover.ReachedTarget(enemy.transform, _currentTarget))
        {
            GameObject nextNode = enemy.Path.GetNextNode();
            if (nextNode != null)
                _currentTarget = nextNode.transform.position;
            else
                enemy.ReachCastle();
        }
    }

    public void Exit(Enemy enemy) { }
}