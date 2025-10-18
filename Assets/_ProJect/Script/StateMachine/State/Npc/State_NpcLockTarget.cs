using UnityEngine;
using UnityEngine.AI;

public class State_NpcLockTarget : AbstractState
{
    [SerializeField] private float speedLockRotation = 5;

    private NavMeshAgent agent;
    private Npc_Controller npc_Controller;

    private bool hasTartget;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State NpcLockTarget");

        if (agent == null) agent = controller.GetComponent<NavMeshAgent>();
        if (npc_Controller == null) npc_Controller = controller.GetComponent<Npc_Controller>();

        hasTartget = npc_Controller.HasTarget;

        agent.updateRotation = !hasTartget;
    }

    public override void StateUpdate() { LockRotation(); }

    private void LockRotation()
    {
        if(hasTartget != npc_Controller.HasTarget) agent.updateRotation = !hasTartget;
        if (!npc_Controller.HasTarget) return;

        Vector3 directionToTarget = npc_Controller.GetTarget().position - controller.transform.position;
        directionToTarget.y = 0;
        if (directionToTarget.sqrMagnitude < 0.01f) return;
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, Quaternion.LookRotation(directionToTarget.normalized), speedLockRotation * Time.deltaTime);
    }

    public override void StateLeave() 
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State NpcLockTarget");
        agent.updateRotation = true;
    }
}
