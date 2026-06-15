using UnityEngine;

public class CharacterControllerMover : IMover
{
    private CharacterController _controller;

    public CharacterControllerMover(CharacterController controller)
    {
        _controller = controller;
    }

    public void MoveTowards(Transform transform, Vector3 target, float speed, float turnSpeed)
    {
        Vector3 direction = (target - transform.position).normalized;
        MoveInDirection(transform, direction, speed, turnSpeed);
    }

    public void MoveInDirection(Transform transform, Vector3 direction, float speed, float turnSpeed)
    {
        Vector3 move = direction * speed * Time.deltaTime;

        Vector3 directionFlat = direction;
        directionFlat.y = 0;
        if (directionFlat != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionFlat);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        _controller.Move(move);
    }

    public bool ReachedTarget(Transform transform, Vector3 target, float threshold = 0.8f)
    {
        Vector3 currentPos = transform.position;
        currentPos.y = 0;
        Vector3 targetPos = target;
        targetPos.y = 0;
        return Vector3.Distance(currentPos, targetPos) <= threshold;
    }
}