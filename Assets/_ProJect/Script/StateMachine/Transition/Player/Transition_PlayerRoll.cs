using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_PlayerRoll : AbstractTransition
{
    [SerializeField] private int costRollStamina = 15;

    [SerializeField] private float delay = 0.25f;
    [SerializeField] private bool isOnDelay;

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
            StopAllCoroutines();
            stamina_Controller.OnUpdateStamina(-costRollStamina);
            conditionMet = true;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        if (isOnDelay)
        {
            StartCoroutine(DelayStartRoutine());
            return;
        }
        else
        {
            if (human_BasicInput != null) human_BasicInput.OnRoll += Roll;
        }   
    }

    private IEnumerator DelayStartRoutine()
    {
        yield return new WaitForSeconds(delay);
        if (human_BasicInput != null) human_BasicInput.OnRoll += Roll;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
        if (human_BasicInput != null) human_BasicInput.OnRoll -= Roll;
    }
}
