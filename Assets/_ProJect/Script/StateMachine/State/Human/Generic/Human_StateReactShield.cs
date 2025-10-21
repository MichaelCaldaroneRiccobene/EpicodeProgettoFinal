using System.Collections;
using UnityEngine;

public class Human_StateReactShield : AbstractState
{
    [Header("Setting StateReactShield")]
    [SerializeField] protected AbstractState nextState;
    [SerializeField] protected float timeShieldReact = 1;
    [SerializeField] protected EffectScreen_SO effectScreen;

    protected Player_Controller playerController;
    protected Human_Basic_Controller human_Basic_Controller;
    protected Human_BasicAnimator basicAnimator;
    protected Shiedl shiedl;
    protected Coroutine coroutineShieldReact;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State Shield");

        if(playerController == null) playerController = controller.GetComponent<Player_Controller>();
        if (basicAnimator == null) basicAnimator = controller.GetComponentInChildren<Human_BasicAnimator>();
        if(human_Basic_Controller == null) human_Basic_Controller = controller.GetComponent<Human_Basic_Controller>();
        if (coroutineShieldReact == null) shiedl = controller.GetComponentInChildren<Shiedl>();
  
        if(!human_Basic_Controller.IsOnBlockMode) coroutineShieldReact = StartCoroutine(ShieldReactRoutine());
    }

    private IEnumerator ShieldReactRoutine()
    {
        if(!shiedl) yield break;

        human_Basic_Controller.IsOnBlockMode = true;

        yield return null;

        if (playerController)
        {
            effectScreen.ShakeCamera(playerController.transform.position);
            effectScreen.ShockEffect(playerController);
        }

        shiedl.OnBlockReact();
        basicAnimator.OnShieldReact(shiedl.BlockIdle);
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
