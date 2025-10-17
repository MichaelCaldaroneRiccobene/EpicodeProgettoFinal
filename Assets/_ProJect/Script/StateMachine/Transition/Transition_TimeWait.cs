using UnityEngine;

public class Transition_TimeWait : AbstractTransition
{
    [SerializeField] private float timeToWait = 1f;

    public override void CheckConditionUpdate() { if (controller.CurrentStateTime >= timeToWait) conditionMet = true;}
}
