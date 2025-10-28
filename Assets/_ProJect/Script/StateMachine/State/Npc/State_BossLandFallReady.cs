using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class State_BossLandFallReady : AbstractState
{
    [SerializeField] private AbstractState nextState;
    [SerializeField] private AudioList_SO impactClip;
    [SerializeField] private float yPosition = 0f;
    [SerializeField] private float moveDuration = 2f;

    private NavMeshAgent agent;
    private Human_BasicAnimator basicAnimator;

    public override void StateEnter()
    {
        if(agent == null) agent = controller.GetComponent<NavMeshAgent>();
        if(basicAnimator == null) basicAnimator = controller.GetComponentInChildren<Human_BasicAnimator>();

        basicAnimator.SetOffOnRootMotion(false);
        agent.enabled = false;

        controller.transform.DOMoveY(yPosition, moveDuration).SetEase(Ease.InCubic).OnComplete(() =>
        {
            CameraShake.Instance.OnCameraShake(controller.transform.position, 0.5f, 2f, 10f);
            controller.SetUpState(nextState);

            ManagerPooling.Instance.GetObjFromPool(ObjTypePoolling.ImpactBoss, controller.transform.position, Quaternion.identity);
            impactClip.PlaySound(controller.transform);
        });
    }

    public override void StateLeave()
    {
        basicAnimator.SetOffOnRootMotion(false);
        agent.enabled = true;
    }
}