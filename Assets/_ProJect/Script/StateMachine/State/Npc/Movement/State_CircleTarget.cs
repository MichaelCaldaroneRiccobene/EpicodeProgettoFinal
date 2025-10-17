using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_CircleTarget : AbstractState
{
    [Range(3,6)] [SerializeField] private float distanceToTarget = 4;
    [SerializeField] private float speedLockRotation = 3;
    [SerializeField] private float velocityCircleToTarget = 2f;

    [SerializeField] private float timeUpdateRoutine = 0.5f;
    [SerializeField] private float stopDistanceToDestination = 1f;

    private NavMeshAgent agent;
    private Npc_Controller npc_Controller;
    private WaitForSeconds waitForSecondsRoutine;

    public override void StateEnter()
    { 
        if (controller.CanSeeDebug) Debug.Log("Entrato in State CircleTarget"); 

        if(agent ==  null) agent = controller.GetComponent<NavMeshAgent>();
        if(npc_Controller == null) npc_Controller = controller.GetComponent<Npc_Controller>();
        if(waitForSecondsRoutine == null) waitForSecondsRoutine = new WaitForSeconds(timeUpdateRoutine);

        agent.ResetPath();
        agent.stoppingDistance = stopDistanceToDestination;
        agent.updateRotation = false;

        velocityCircleToTarget = UnityEngine.Random.Range(0,2) == 0 ? velocityCircleToTarget * -1 : velocityCircleToTarget;
        StartCoroutine(CircleTargetRoutine());
    }

    public override void StateUpdate() => LockRotation();

    private void LockRotation()
    {
        if (!npc_Controller.HasTarget) return;

        Vector3 directionToTarget = npc_Controller.GetTarget().position - controller.transform.position;
        directionToTarget.y = 0;
        if (directionToTarget.sqrMagnitude < 0.01f) return;
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation,Quaternion.LookRotation(directionToTarget.normalized) ,speedLockRotation * Time.deltaTime);
    }

    private IEnumerator CircleTargetRoutine()
    {
        if(!npc_Controller.HasTarget) yield break;

        Vector3 offSet = agent.transform.position - npc_Controller.GetTarget().position;
        float angle = Mathf.Atan2(offSet.z, offSet.x);

        yield return null;

        while (npc_Controller.HasTarget)
        {
            angle += velocityCircleToTarget * Time.deltaTime;
            float x = Mathf.Cos(angle) * distanceToTarget;
            float z = Mathf.Sin(angle) * distanceToTarget;

            Vector3 destination = new Vector3(npc_Controller.GetTarget().position.x + x, npc_Controller.GetTarget().position.y, npc_Controller.GetTarget().position.z + z);

            agent.SetDestination(destination);

            while (agent.pathPending) yield return null;
            while (agent.remainingDistance > agent.stoppingDistance) { yield return waitForSecondsRoutine; }

            yield return null;
        }
    }

    public override void StateLeave() 
    { 
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State CircleTarget");

        StopAllCoroutines();
        agent.ResetPath();
        agent.updateRotation = true;
    }
}
