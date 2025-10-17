public class Transition_OnDead : AbstractTransition
{

    private LifeController lifeController;

    public override void SetUp(FSM_Controller controller)
    {
        base.SetUp(controller);
        lifeController = controller.GetComponent<LifeController>();
    }
    public override void CheckConditionUpdate() { if(lifeController.IsDead()) conditionMet = true; }
}
