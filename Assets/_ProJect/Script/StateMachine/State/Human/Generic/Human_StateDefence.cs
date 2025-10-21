using UnityEngine;

public class Human_StateDefence : AbstractState
{
    protected Shiedl shiedl;
    protected Human_Basic_Controller human_Basic_Controller;
    protected Human_BasicInput basicInput;
    protected Human_BasicAnimator basicAnimator;

    protected bool isOnBlock;

    public override void StateEnter() 
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State Defence");

        if(shiedl == null) shiedl = controller.GetComponentInChildren<Shiedl>();
        if(human_Basic_Controller == null) human_Basic_Controller = controller.GetComponent<Human_Basic_Controller>();

        if (basicInput == null)
        {
            basicInput = controller.GetComponent<Human_BasicInput>();
            basicInput.OnDefence += (value) => isOnBlock = value;
        }
        if (basicAnimator == null) basicAnimator = controller.GetComponentInChildren<Human_BasicAnimator>();
    }

    public override void StateUpdate() 
    {
        if (shiedl)
        {
            basicAnimator.OnShieldIdle(isOnBlock,shiedl.BlockIdle);
            human_Basic_Controller.IsOnBlockMode = isOnBlock;
        }
    }

    public virtual void OnDisable()
    {
        if (basicInput != null) basicInput.OnDefence -= (value) => isOnBlock = value;
    }

    public override void StateLeave()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State Defence"); 

        isOnBlock = false;
        human_Basic_Controller.IsOnBlockMode = isOnBlock;

        basicAnimator.OnShieldIdle(isOnBlock, shiedl.BlockIdle);
    }
}
