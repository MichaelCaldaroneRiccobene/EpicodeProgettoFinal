using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class State_FollowTarget : AbstractState
{
    [Header("Setting FollowEntity")]
    [SerializeField] private float timeUpdateSightRoutine = 0.2f;
    [SerializeField] private float stopDistanceToDestination = 2f;

    private Npc_Controller npc_Controller;
    private NavMeshAgent agent;
    private WaitForSeconds waitForSeconds;

    public override void StateEnter() 
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State FollowEntity");

        if (npc_Controller == null) npc_Controller = controller.GetComponent<Npc_Controller>();
        if (agent == null) agent = controller.GetComponent<NavMeshAgent>();
        if(waitForSeconds == null) waitForSeconds = new WaitForSeconds(timeUpdateSightRoutine);

        agent.ResetPath();

        StartCoroutine(FollowTargetRoutine());
    }
    private IEnumerator FollowTargetRoutine()
    {
        yield return null;

        agent.stoppingDistance = stopDistanceToDestination;
        agent.ResetPath();

        while (true)
        {
            if (!npc_Controller.HasTarget) break;

            Vector3 positionToFollow = npc_Controller.GetTarget().position;
            if (NavMesh.SamplePosition(positionToFollow, out NavMeshHit hit, 2f, NavMesh.AllAreas)) positionToFollow = hit.position;

            agent.SetDestination(positionToFollow);
            while (agent.pathPending) yield return null;

            while (agent.remainingDistance > agent.stoppingDistance) yield return waitForSeconds;

            yield return null;
        }
    }

    public override void StateLeave()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State FollowEntity");

        StopAllCoroutines();
        if(agent.enabled) agent.ResetPath();
    }
}
