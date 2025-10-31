using UnityEngine;

public class Transition_OnHit : AbstractTransition
{
    [SerializeField] private AbstractState transitionShieldBlock;
    [SerializeField] private int costStaminaShiedlBlock = 20;

    private Human_Basic_Controller human_Basic_Controller;
    private LifeController lifeController;
    private Stamina_Controller stamina_Controller;
    private bool isHit;
    private bool iHit;

    public override void SetUp(FSM_Controller controller)
    {
        base.SetUp(controller);

        lifeController = controller.GetComponent<LifeController>();
        stamina_Controller = controller.GetComponent<Stamina_Controller>();
        human_Basic_Controller = controller.GetComponent<Human_Basic_Controller>();

        if(lifeController) lifeController.FisicalDamage += FisicalDamage;
    }

    public override void CheckConditionUpdate() { if (human_Basic_Controller.IsOnNotHitReact) return; if (isHit) conditionMet = true; }

    public virtual void FisicalDamage(int value, Vector3 hitPoint)
    {
        if (human_Basic_Controller.IsOnNotHitReact)
        {
            TakeDamage(value, false);
            return;
        }

        if (human_Basic_Controller.IsOnBlockMode)
        {
            Vector3 hitDirection = (hitPoint - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, hitDirection);

            if (dot > 0f && stamina_Controller.CheckCanUseStamina(costStaminaShiedlBlock))
            {
                controller.SetUpState(transitionShieldBlock);
                stamina_Controller.OnUpdateStamina(-costStaminaShiedlBlock);
            }
            else TakeDamage(value, true);
        }
        else TakeDamage(value,true);
    }

    public virtual void TakeDamage(int value,bool condition)
    {
        if(iHit) return;
        iHit = true;
        if (lifeController != null) lifeController.OnUpdateLife(value);
        else Debug.LogWarning("No Life controller ", transform);

        conditionMet = condition;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        if (lifeController != null) lifeController.FisicalDamage += FisicalDamage;

        iHit = false;
        isHit = false;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (lifeController != null) lifeController.FisicalDamage -= FisicalDamage;

        iHit = false;
        isHit = false;
    }
}
