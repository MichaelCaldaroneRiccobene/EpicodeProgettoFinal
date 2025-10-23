using System.Collections;
using UnityEngine;

public class Human_StateHit : AbstractState
{
    [Header("Setting StateHit")]
    [SerializeField] protected AbstractState nextState;
    [SerializeField] protected float timeAnimation = 0.96f;
    [SerializeField] protected EffectScreen_SO effectScreen;


    protected Player_Controller player_Controller;
    protected Human_BasicAnimator basicAnimator;
    protected Coroutine coroutineHit;
    protected bool isOnHitRoutin;

    public override void StateEnter() 
    { 
        if (controller.CanSeeDebug) Debug.Log("Entrato in State Hit");

        if (player_Controller == null) player_Controller = controller.GetComponent<Player_Controller>();
        if(basicAnimator == null) basicAnimator = controller.GetComponentInChildren<Human_BasicAnimator>();

        if(!isOnHitRoutin) coroutineHit = StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        isOnHitRoutin = true;

        yield return null;

        if (player_Controller)
        {
            effectScreen.ShockEffect(player_Controller);
            effectScreen.ShakeCamera(player_Controller.transform.position);
        }
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
