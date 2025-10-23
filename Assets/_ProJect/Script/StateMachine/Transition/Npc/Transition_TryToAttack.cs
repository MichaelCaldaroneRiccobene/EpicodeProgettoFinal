using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Transition_TryToAttack : AbstractTransition
{
    [SerializeField] private NpcMode mode;
    [SerializeField] private float minTimeToWaitBeforeAttackTarget = 3;
    [SerializeField] private float maxTimeToWaitBeforeAttackTarget = 10;

    [SerializeField] private DistanceCondition distanceCondition;
    [SerializeField] private float distanceToCheck;

    private Npc_Controller npc_Controller;
    private Stamina_Controller stamina_Controller;

    private float staminaPercenter;
    private float timeWait;

    public override void SetUp(FSM_Controller controller)
    {
        base.SetUp(controller);
        if(stamina_Controller == null) stamina_Controller = controller.GetComponent<Stamina_Controller>();
        if(npc_Controller == null) npc_Controller = controller.GetComponent<Npc_Controller>();

        SetNpcMode();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        timeWait = Random.Range(minTimeToWaitBeforeAttackTarget, maxTimeToWaitBeforeAttackTarget);
    }

    private void SetNpcMode()
    {
        switch (mode)
        {
            case NpcMode.Passive:
                staminaPercenter = 0.8f;
                break;
            case NpcMode.Normal:
                staminaPercenter = 0.5f;
                break;
            case NpcMode.Aggressiv:
                staminaPercenter = 0.2f;
                break;
        }
    }

    public override void CheckConditionUpdate()
    {
        if(!npc_Controller.HasTarget) return;

        if(!npc_Controller.GetTarget().TryGetComponent(out I_Token token)) return;
        if (token.GetToken() <= 0) return;

        float distance = Vector3.Distance(transform.position, npc_Controller.GetTarget().position);

        switch (distanceCondition)
        {
            case DistanceCondition.LessThan:
                if (distance < distanceToCheck && staminaPercenter < stamina_Controller.GetPercentStamina() && controller.CurrentStateTime > timeWait) conditionMet = true;
                break;
            case DistanceCondition.GreaterThan:
                if (distance > distanceToCheck && staminaPercenter < stamina_Controller.GetPercentStamina() && controller.CurrentStateTime > timeWait) conditionMet = true;
                break;
        }
    }
}
