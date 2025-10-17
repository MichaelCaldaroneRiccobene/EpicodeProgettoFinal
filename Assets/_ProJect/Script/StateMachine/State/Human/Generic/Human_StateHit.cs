using System.Collections;
using UnityEngine;

public class Human_StateHit : AbstractState
{
    [Header("Setting StateHit")]
    [SerializeField] protected AbstractState nextState;
    [SerializeField] protected float timeAnimation = 0.96f;

    protected Human_BasicAnimator basicAnimator;
    protected Coroutine coroutineHit;
    protected bool isOnHitRoutin;

    public override void StateEnter() 
    { 
        if (controller.CanSeeDebug) Debug.Log("Entrato in State Hit");

        if(basicAnimator == null) basicAnimator = controller.GetComponentInChildren<Human_BasicAnimator>();

        if(!isOnHitRoutin) coroutineHit = StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        isOnHitRoutin = true;

        yield return null;
        basicAnimator.OnHitReact();
        basicAnimator.SetOffOnRootMotion(true);

        yield return new WaitForSeconds(timeAnimation);
  
        controller.SetUpState(nextState);
    }

    public override void StateLeave() 
    { 
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State Hit");

        if(coroutineHit != null) StopCoroutine(coroutineHit);

        basicAnimator.SetOffOnRootMotion(false);
        isOnHitRoutin = false;
    }
}
