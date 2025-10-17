using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Dead : AbstractState
{
    [SerializeField] private float timeToWaiteAnimationDead = 1f;
    private Human_BasicAnimator basicAnimator;

    public override void StateEnter() 
    { 
        if (controller.CanSeeDebug) Debug.Log("Entrato in State Dead");
        if(basicAnimator == null) basicAnimator = controller.GetComponentInChildren<Human_BasicAnimator>();

        basicAnimator.ReturnIdle();
        StartCoroutine(DeadRoutine());
    }

    private IEnumerator DeadRoutine()
    {
        yield return new WaitForSeconds(timeToWaiteAnimationDead);
        Destroy(controller.gameObject);
    }


    public override void StateLeave() { if (controller.CanSeeDebug) Debug.Log("Uscito dallo State Dead"); }
}
