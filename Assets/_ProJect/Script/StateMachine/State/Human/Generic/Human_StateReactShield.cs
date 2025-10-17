using System.Collections;
using UnityEngine;

public class Human_StateReactShield : AbstractState
{
    [Header("Setting StateReactShield")]
    [SerializeField] protected AbstractState nextState;
    [SerializeField] protected float timeShieldReact = 1;

    protected Human_Basic_Controller human_Basic_Controller;
    protected Human_BasicAnimator basicAnimator;
    protected Coroutine coroutineShieldReact;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State Shield");

        if(basicAnimator == null) basicAnimator = controller.GetComponentInChildren<Human_BasicAnimator>();
        if(human_Basic_Controller == null) human_Basic_Controller = controller.GetComponent<Human_Basic_Controller>();
  
        if(!human_Basic_Controller.IsOnBlockMode) coroutineShieldReact = StartCoroutine(ShieldReactRoutine());
    }

    private IEnumerator ShieldReactRoutine()
    {
        human_Basic_Controller.IsOnBlockMode = true;

        yield return null;
        basicAnimator.OnShieldReact();
        basicAnimator.SetOffOnRootMotion(true);

        yield return new WaitForSeconds(timeShieldReact);

        controller.SetUpState(nextState);
    }

    public override void StateLeave() 
    { 
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State Shield");

        if (coroutineShieldReact != null) StopCoroutine(coroutineShieldReact);
        human_Basic_Controller.IsOnBlockMode = false;
    }
}
