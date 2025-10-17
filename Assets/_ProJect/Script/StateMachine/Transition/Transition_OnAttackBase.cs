
public class Transition_OnAttackBase : AbstractTransition
{
    protected Stamina_Controller stamina_Controller;
    protected Human_BasicInput human_BasicInput;
    protected Human_BasicIteamEquip iteamEquip;

    public override void SetUp(FSM_Controller controller)
    {
        base.SetUp(controller);
        human_BasicInput = controller.GetComponent<Human_BasicInput>();
        human_BasicInput.OnAttack += Attack;

        stamina_Controller = controller.GetComponent<Stamina_Controller>(); 
        iteamEquip = controller.GetComponent<Human_BasicIteamEquip>();
    }

    public override void CheckConditionUpdate() { }

    private void Attack()
    {
       if(stamina_Controller.CheckCanUseStamina(iteamEquip.Weapon.CostStaminaAttackBase)) conditionMet = true;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        if(human_BasicInput != null) human_BasicInput.OnAttack += Attack;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (human_BasicInput != null) human_BasicInput.OnAttack -= Attack;
    }
}
