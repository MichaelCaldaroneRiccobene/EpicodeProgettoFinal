using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_OnCheckToBlockOrRoll1 : AbstractState
{
    [SerializeField] private float shieldDurationReactToAttack = 1.5f;
    [Range(0f, 9f)] [SerializeField] private int rangeChanceDefence = 5;

    private Shiedl shiedl;
    private Human_BasicAnimator basicAnimator;
    private Npc_Controller npc_Controller;
    private NavMeshAgent agent;

    private bool isOnShieldBlock;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State OnCheckToBlockOrRoll");

        if (npc_Controller == null) npc_Controller = controller.GetComponent<Npc_Controller>();
        if (basicAnimator == null) basicAnimator = controller.GetComponentInChildren<Human_BasicAnimator>();
        if(agent == null) agent = controller.GetComponent<NavMeshAgent>();
        if(shiedl == null) shiedl = npc_Controller.GetComponentInChildren<Shiedl>();

        isOnShieldBlock = false;
    }

    public override void StateUpdate()
    {
        if (!npc_Controller.HasTarget) return;

        if (npc_Controller.GetTarget().TryGetComponent(out I_Attack attack))
        {
            if (attack.GetIsAttack() && !isOnShieldBlock)
            {
                if(Random.Range(0,12) > rangeChanceDefence) StartCoroutine(WaitToBeHitRoutine());
                else StartCoroutine(ShieldOnRoutine());
            }
        }

        if(isOnShieldBlock)
        {
            Vector3 directionToTarget = npc_Controller.GetTarget().position - controller.transform.position;
            directionToTarget.y = 0;
            if (directionToTarget.sqrMagnitude < 0.01f) return;
            controller.transform.rotation = Quaternion.LookRotation(directionToTarget.normalized);
        }
    }

    private IEnumerator ShieldOnRoutine()
    {
        isOnShieldBlock = true;
        yield return new WaitForSeconds(0.2f);
        EnableDisableBlockMode(true);

        yield return new WaitForSeconds(shieldDurationReactToAttack);

        isOnShieldBlock = false;
        EnableDisableBlockMode(false);
    }

    private IEnumerator WaitToBeHitRoutine()
    {
        isOnShieldBlock = true;
        yield return new WaitForSeconds(shieldDurationReactToAttack * 2);
        isOnShieldBlock = false;
    }

    private void EnableDisableBlockMode(bool value)
    {
        if(!shiedl) return;
        agent.updateRotation = !value;
        basicAnimator.OnShieldIdle(value, shiedl.BlockIdle);
        npc_Controller.IsOnBlockMode = value;
    }

    public override void StateLeave()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State OnCheckToBlockOrRoll");
        StopAllCoroutines();
        EnableDisableBlockMode(false);
        isOnShieldBlock = false;
    }
}
