using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_OnLostSeeTarget : AbstractTransition
{
    [Header("Setting OnLostSightEntity")]
    [SerializeField] private float hight = 1;
    [SerializeField] private float sightDistance = 12;
    [SerializeField] private float timeForLostSightEnemy = 10;

    private Npc_Controller npc_Controller;

    private float timerForLostSightTarget;
    private float lastTimeCheck;

    public override void SetUp(FSM_Controller controller)
    {
        base.SetUp(controller);
        if(npc_Controller == null) npc_Controller = controller.GetComponent<Npc_Controller>();
    }

    public override void CheckConditionUpdate() { if (SeeTarget(controller)) conditionMet = true; }

    private bool SeeTarget(FSM_Controller controller)
    {
        if(!npc_Controller.HasTarget) return true;

        // (Se target muore vado via)
        if (npc_Controller.HasTarget)
        {
            if (npc_Controller.GetTarget().TryGetComponent(out LifeController lifeController))
            {
                if (lifeController.IsDead())
                {
                    OnLostTarget();
                    return true;
                }
            }
        }

        Vector3 originCast = transform.position + new Vector3(0, hight, 0);
        Vector3 targetOriginCast = npc_Controller.GetTarget().position + new Vector3(0, hight, 0);
        Vector3 direction = targetOriginCast - originCast;

        if (Physics.Raycast(originCast, direction, out RaycastHit hit, sightDistance))
        {
            if (controller.CanSeeDebug) Debug.DrawLine(originCast, hit.point, Color.red, 1);

            if (hit.transform == npc_Controller.GetTarget()) timerForLostSightTarget = 0;
            else timerForLostSightTarget += Time.time - lastTimeCheck;
        }
        else timerForLostSightTarget += Time.time - lastTimeCheck;

        lastTimeCheck = Time.time;

        if (timerForLostSightTarget >= timeForLostSightEnemy)
        {
            OnLostTarget();
            return true;
        }
        return false;
    }


    private void OnLostTarget()
    {
        timerForLostSightTarget = 0;
        npc_Controller.SetTarget(null);
    }
}
