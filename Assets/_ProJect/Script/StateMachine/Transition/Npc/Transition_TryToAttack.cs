using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_TryToAttack : AbstractTransition
{
    [SerializeField] private float timeToWaitBeforeAttackTarget = 5;

    public override void CheckConditionUpdate()
    {
        if (controller.CurrentStateTime > timeToWaitBeforeAttackTarget) conditionMet = true;
    }
}
