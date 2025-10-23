using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_WalkUpDownToTarget : AbstractState
{
    [Header("Setting WalkUpDownToTarget")]
    [SerializeField] private float timeUpdateRoutine = 1f;
    [SerializeField] private float stopDistanceToDestination = 2f;
    [SerializeField] private float distanceOnDown = 3f;
    [SerializeField] private float distanceOnUp = 7f;


    private Npc_Controller npc_Controller;
    private NavMeshAgent agent;
    private bool isOnFoward;    


    public override void StateEnter() 
    { 
        if (controller.CanSeeDebug) Debug.Log("Entrato in State WalkUpDownToTarget");

        if (agent == null) agent = controller.GetComponent<NavMeshAgent>();
        if(npc_Controller == null) npc_Controller = controller.GetComponent<Npc_Controller>();

        agent.ResetPath();
        StartCoroutine(GoOnPositionRoutine());
    }

    private IEnumerator GoOnPositionRoutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);
        agent.stoppingDistance = stopDistanceToDestination;
        yield return null;

        while (true)
        {
            if(!npc_Controller.HasTarget) yield break;

            Vector3 direction = (agent.transform.position - npc_Controller.GetTarget().position).normalized;

            Vector3 position = isOnFoward? npc_Controller.GetTarget().position + direction * distanceOnUp : npc_Controller.GetTarget().position + direction * distanceOnDown;


            if (NavMesh.SamplePosition(position, out NavMeshHit hit, 2f, NavMesh.AllAreas)) position = hit.position;

            agent.SetDestination(position);
            while (agent.pathPending) yield return null;

            while (agent.remainingDistance > agent.stoppingDistance) { yield return waitForSeconds; }

            isOnFoward = !isOnFoward;
            yield return waitForSeconds;
        }
    }

    public override void StateLeave()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State WalkUpDownToTarget");

        StopAllCoroutines();
        agent.ResetPath();
    }
}
