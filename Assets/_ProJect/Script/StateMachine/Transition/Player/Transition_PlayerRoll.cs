using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_PlayerRoll : AbstractTransition
{
    [SerializeField] private int costRollStamina = 15;

    protected Human_BasicInput human_BasicInput;
    protected Stamina_Controller stamina_Controller;

    public override void SetUp(FSM_Controller controller)
    {
        base.SetUp(controller);
        human_BasicInput = controller.GetComponent<Human_BasicInput>();
        human_BasicInput.OnRoll += Roll;

        stamina_Controller = controller.GetComponent<Stamina_Controller>();
    }

    public override void CheckConditionUpdate() { }

    private void Roll()
    {
        if (stamina_Controller.CheckCanUseStamina(costRollStamina))
        {
            stamina_Controller.OnUpdateStamina(-costRollStamina);
            conditionMet = true;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        if (human_BasicInput != null) human_BasicInput.OnRoll += Roll;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (human_BasicInput != null) human_BasicInput.OnRoll -= Roll;
    }
}
