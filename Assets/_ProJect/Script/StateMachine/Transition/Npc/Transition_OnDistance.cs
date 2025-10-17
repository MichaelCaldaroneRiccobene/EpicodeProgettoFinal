using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DistanceCondition { LessThan, GreaterThan }
public class Transition_OnDistance : AbstractTransition
{
    [SerializeField] private DistanceCondition distanceCondition;
    [SerializeField] private float distanceToCheck;

    private Npc_Controller npc_Controller;

    public override void SetUp(FSM_Controller controller)
    {
        base.SetUp(controller);
        if(npc_Controller == null) npc_Controller = controller.GetComponent<Npc_Controller>();
    }

    public override void CheckConditionUpdate()
    {
        if(!npc_Controller.HasTarget) return;
        float distance = Vector3.Distance(transform.position, npc_Controller.GetTarget().position);

        switch (distanceCondition)
        {
            case DistanceCondition.LessThan:
                if (distance < distanceToCheck) conditionMet = true;
                break;
            case DistanceCondition.GreaterThan:
                if (distance > distanceToCheck) conditionMet = true;
                break;
        }
    }
}
