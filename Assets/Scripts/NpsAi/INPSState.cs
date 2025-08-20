using UnityEngine;

public interface INPSState
{
    void Enter(NpsBehaviorLogic controller);
    void Update(NpsBehaviorLogic controller);
    void Exit(NpsBehaviorLogic controller);
}


public class WalkState : INPSState
{
    public void Enter(NpsBehaviorLogic controller)
    {
        if (NPSMovePoints.PointsInstance != null)
        {
            controller.CurrentDestination = NPSMovePoints.PointsInstance.TakeRandPoint();

            if (controller.CurrentDestination == null) controller.ChangeState(new IdleState());

            controller.Animator.SetInteger(controller.IntegerName, 1);
        }
    }

    public void Exit(NpsBehaviorLogic controller)
    {
        controller.ChangeState(new IdleState());
    }

    public void Update(NpsBehaviorLogic controller)
    {
        if (controller.Agent != null && controller.CurrentDestination != null)
        {
            if (Vector3.Distance(controller.gameObject.transform.position, controller.CurrentDestination.transform.position) <= 0.1f) Exit(controller);
            controller.Agent.SetDestination(controller.CurrentDestination.position);
        }
    }
}

public class IdleState : INPSState
{
    public void Enter(NpsBehaviorLogic controller)
    {
        controller.Animator.SetInteger(controller.IntegerName, 0);
    }

    public void Exit(NpsBehaviorLogic controller) { }

    public void Update(NpsBehaviorLogic controller) { }
}

public class TalkState : INPSState
{
    public void Enter(NpsBehaviorLogic controller)
    {
        controller.Animator.SetInteger(controller.IntegerName, 2);
    }

    public void Exit(NpsBehaviorLogic controller) { }

    public void Update(NpsBehaviorLogic controller) { }
}
