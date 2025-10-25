using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class State_BossLandFallReady : AbstractState
{
    [SerializeField] private AnimatorOverrideController landFallAnimator;
    [SerializeField] private AbstractState nextState;
    [SerializeField] private float yPosition = 0f;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private GameObject landImpact;

    private NavMeshAgent agent;
    private Human_BasicAnimator basicAnimator;

    public override void StateEnter()
    {
        if(agent == null) agent = controller.GetComponent<NavMeshAgent>();
        if(basicAnimator == null) basicAnimator = controller.GetComponentInChildren<Human_BasicAnimator>();

        basicAnimator.SetOffOnRootMotion(false);
        agent.enabled = false;

        controller.transform.DOMoveY(yPosition, moveDuration).OnComplete(() =>
        {          
            controller.SetUpState(nextState);
        });
    }

    public override void StateLeave()
    {
        basicAnimator.SetOffOnRootMotion(false);
        agent.enabled = true;
    }
}