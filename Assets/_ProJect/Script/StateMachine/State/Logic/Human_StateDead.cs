using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human_StateDead : AbstractState
{
    [SerializeField] private float timeToWaiteAnimationDead = 1f;
    private Human_BasicAnimator basicAnimator;

    public override void StateEnter() 
    { 
        if (controller.CanSeeDebug) Debug.Log("Entrato in State Dead");
        if(basicAnimator == null) basicAnimator = controller.GetComponentInChildren<Human_BasicAnimator>();

        basicAnimator.OnDeath();
        StartCoroutine(DeadRoutine());
    }

    private IEnumerator DeadRoutine()
    {
        yield return new WaitForSeconds(timeToWaiteAnimationDead);
        controller.gameObject.SetActive(false);
    }


    public override void StateLeave() { if (controller.CanSeeDebug) Debug.Log("Uscito dallo State Dead"); }
}
