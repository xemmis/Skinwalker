using System.Collections;
using UnityEngine;

public interface INPSState
{
    void Enter(NpsBehaviorLogic controller);
    void Update(NpsBehaviorLogic controller);
    void Exit(NpsBehaviorLogic controller);
}


public class WalkState : INPSState
{
    private float _rotationSpeed = 5f;

    private bool _isRotating = true;

    public void Enter(NpsBehaviorLogic controller)
    {
        if (NPSMovePoints.PointsInstance != null)
        {
            if (!controller.IsGoTalk)
            {
                controller.CurrentDestination = NPSMovePoints.PointsInstance.TakeRandPoint();

            }
            else
            {
                controller.CurrentDestination = NPSMovePoints.PointsInstance.PointToDialogue;
            }

            if (controller.CurrentDestination == null) controller.ChangeState(new IdleState());
            controller.Animator.SetInteger(controller.IntegerName, 1);
        }
    }

    public void Exit(NpsBehaviorLogic controller)
    {
        controller.Agent.ResetPath();
    }

    public void Update(NpsBehaviorLogic controller)
    {
        if (controller.CurrentDestination == null) return;

        Vector3 direction = (controller.CurrentDestination.position - controller.transform.position).normalized;
        direction.y = 0;

        float distance = Vector3.Distance(controller.transform.position, controller.CurrentDestination.position);
        Debug.LogWarning(distance);
        if (distance < 1.35f)
        {
            if (controller.IsGoTalk)
                controller.ChangeState(new TalkState());
            else
                controller.ChangeState(new IdleState());
        }

        if (_isRotating)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            controller.transform.rotation = Quaternion.Slerp(
                controller.transform.rotation,
                targetRotation,
                _rotationSpeed * Time.deltaTime
            );

            // Начинаем движение только когда почти повернулись
            if (Quaternion.Angle(controller.transform.rotation, targetRotation) < 10f)
            {
                _isRotating = false;
                controller.Agent.SetDestination(controller.CurrentDestination.position);
            }
        }
        else
        {
            controller.Agent.SetDestination(controller.CurrentDestination.position);
        }
    }

}


public class IdleState : INPSState
{
    private float _timer;
    private Quaternion _targetRotation;
    private float _rotationSpeed = 2f;

    public void Enter(NpsBehaviorLogic controller)
    {
        controller.Animator.SetInteger(controller.IntegerName, 0);
        _timer = 0f;

        // Вычисляем направление взгляда
        if (controller.CurrentDestination != null)
        {
            Vector3 direction = controller.CurrentDestination.position - controller.transform.position;
            direction.y = 0; // Игнорируем разницу по высоте
            _targetRotation = Quaternion.LookRotation(direction);
        }
        else
        {
            _targetRotation = controller.transform.rotation; // Оставляем текущий поворот
        }
    }

    public void Exit(NpsBehaviorLogic controller) { }

    public void Update(NpsBehaviorLogic controller)
    {
        // Плавный поворот к цели
        controller.transform.rotation = Quaternion.Slerp(
            controller.transform.rotation,
            _targetRotation,
            _rotationSpeed * Time.deltaTime
        );

        _timer += Time.deltaTime;

        if (_timer >= Random.Range(1, 3))
        {
            controller.IsGoTalk = Random.Range(0, 100) < 60;
            controller.ChangeState(new WalkState());
        }
    }
}

public class TalkState : INPSState
{
    private Transform _player;
    private float _rotationSpeed = 3f;

    public void Enter(NpsBehaviorLogic controller)
    {
        controller.Animator.SetInteger(controller.IntegerName, 2);
        _player = NPSMovePoints.PointsInstance.PlayerPoint;
    }

    public void Exit(NpsBehaviorLogic controller) { }

    public void Update(NpsBehaviorLogic controller)
    {
        if (_player != null)
        {
            Vector3 direction = _player.position - controller.transform.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            controller.transform.rotation = Quaternion.Slerp(
                controller.transform.rotation,
                targetRotation,
                _rotationSpeed * Time.deltaTime
            );
        }
    }
}
