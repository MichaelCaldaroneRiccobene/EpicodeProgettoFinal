using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Human_StateDead : AbstractState
{
    [SerializeField] private float timeToWaiteAnimationDead = 1f;
    private Human_BasicAnimator basicAnimator;

    public UnityEvent OnDead;

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
        OnDead?.Invoke();
        controller.gameObject.SetActive(false);
    }


    public override void StateLeave() { if (controller.CanSeeDebug) Debug.Log("Uscito dallo State Dead"); }
}
