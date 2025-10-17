using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Npc_Animator : Human_BasicAnimator
{
    protected NavMeshAgent agent;

    public override void Awake()
    {
        base.Awake();
        agent = GetComponentInParent<NavMeshAgent>();
    }

    #region AnimationMoving
    public override void AnimationMoving(float currentSpeed, float maxSpeed)
    {
        if (animator == null) return;

        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);

        float vertical = localVelocity.z;
        float horizontal = localVelocity.x;
        float normalizedSpeed = Mathf.Clamp01(currentSpeed / maxSpeed);

        vertical = Mathf.Clamp(vertical, -1f, 1f);
        float speedVertical = normalizedSpeed * vertical;
        animator.SetFloat(parameterFloatSpeed, speedVertical, smoothAnimation, Time.deltaTime);

        horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        float speedHorizontal = normalizedSpeed * horizontal;
        animator.SetFloat(parameterFloatDirection, speedHorizontal, smoothAnimation, Time.deltaTime);
    }
    #endregion

    #region Setting General
    public override void OnMoveWithRootMotion()
    {
        Vector3 horizontalDelta = new Vector3(animator.deltaPosition.x, 0, animator.deltaPosition.z);
        Vector3 finalDelta = horizontalDelta + Vector3.up * agent.velocity.y * Time.deltaTime;

        agent.Move(animator.deltaPosition);
        transform.rotation *= animator.deltaRotation;
    }
    #endregion
}
