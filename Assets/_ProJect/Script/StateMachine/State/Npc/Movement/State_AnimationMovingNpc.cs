using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_AnimationMovingNpc : AbstractState
{
    [SerializeField] private VelocityNpc selectVelocity;

    private NavMeshAgent agent;
    private Npc_Controller npc_Controller;
    private Human_BasicAnimator basicAnimator;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State AnimationMovingNpc");
        if (agent == null) agent = controller.GetComponent<NavMeshAgent>();
        if (npc_Controller == null) npc_Controller = controller.GetComponent<Npc_Controller>();
        if (basicAnimator == null) basicAnimator = controller.GetComponentInChildren<Human_BasicAnimator>();

        StartCoroutine(WaitOneFrameRoutine());
    }

    public override void StateUpdate() => basicAnimator.AnimationMoving(agent.speed, npc_Controller.JogSpeed);

    private void SelectSpeed()
    {
        switch (selectVelocity)
        {
            case VelocityNpc.Walk:
                agent.speed = npc_Controller.WalkSpeed;
                break;
            case VelocityNpc.Jog:
                agent.speed = npc_Controller.JogSpeed;
                break;
        }
    }

    private IEnumerator WaitOneFrameRoutine()
    {
        yield return null;
        basicAnimator.SetOffOnRootMotion(false);
        basicAnimator.ReturnIdle();

        SelectSpeed();
    }

    public override void StateLeave() 
    { 
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State AnimationMovingNpc"); 
        StopAllCoroutines();
    }
}
