using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_PlayerRoll : AbstractTransition
{
    protected Human_BasicInput human_BasicInput;

    public override void SetUp(FSM_Controller controller)
    {
        base.SetUp(controller);
        human_BasicInput = controller.GetComponent<Human_BasicInput>();
        human_BasicInput.OnRoll += Roll;
    }

    public override void CheckConditionUpdate() { }

    private void Roll() => conditionMet = true;

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
