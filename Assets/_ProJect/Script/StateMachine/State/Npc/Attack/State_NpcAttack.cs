using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcMode { Passive,Normal,Aggressiv}

public class State_NpcAttack : AbstractState
{
    [Header("Setting StateAttack")]
    [SerializeField] protected AbstractState nextState;
    [SerializeField] private NpcMode mode;

    protected Human_Basic_Controller human_Basic_Controller;
    protected Human_BasicAnimator basicAnimator;
    protected Human_BasicIteamEquip iteamEquip;

    protected Stamina_Controller stamina_Controller;

    private int attackToDo;
    protected int comboNumber;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State NpcAttack ");

        if (human_Basic_Controller == null) human_Basic_Controller = controller.GetComponent<Human_Basic_Controller>();

        if (basicAnimator == null) basicAnimator = controller.GetComponentInChildren<Human_BasicAnimator>();
        if (iteamEquip == null) iteamEquip = controller.GetComponent<Human_BasicIteamEquip>();
        if (stamina_Controller == null) stamina_Controller = controller.GetComponent<Stamina_Controller>();

        SelectModeNpc();
        ResetCombo();

        OnAttack();
    }

    private void SelectModeNpc()
    {
        switch (mode)
        {
            case NpcMode.Passive:
                attackToDo = Random.Range(1, 3);
                break;
            case NpcMode.Normal:
                attackToDo = Random.Range(2, 4);
                break;
            case NpcMode.Aggressiv:
                attackToDo = Random.Range(3, 7);
                break;
        }
    }

    public virtual void OnAttack() => StartCoroutine(AttackRoutine());

    private void ProcessAttack()
    {
        human_Basic_Controller.IsOnNotHitReact = false;

        basicAnimator.SetOffOnRootMotion(true);
        basicAnimator.OnAttackMeeleName(iteamEquip.Weapon.AttacksList.AttackParameters[comboNumber].AnimatorOverrideController);

        iteamEquip.Weapon.DamageWepon = iteamEquip.Weapon.AttacksList.AttackParameters[comboNumber].Damage;
        comboNumber++;

        if (comboNumber >= iteamEquip.Weapon.AttacksList.AttackParameters.Count)
        {
            human_Basic_Controller.IsOnNotHitReact = true;
            comboNumber = 0;
        }
    }

    private IEnumerator AttackRoutine()
    {
        yield return null;

        for (int i = 0; i < attackToDo; i++)
        {
            if (!stamina_Controller.CheckCanUseStamina(iteamEquip.Weapon.CostStaminaAttackBase))
            {
                OnLeave();
                yield break;
            }

            yield return null;

            ProcessAttack();

            yield return null;

            human_Basic_Controller.SetIsAttack(true);
            while (!basicAnimator.IsFinishAttack) yield return null;
            stamina_Controller.OnUpdateStamina(-iteamEquip.Weapon.CostStaminaAttackBase);
            human_Basic_Controller.SetIsAttack(false);
            RestorCanAttack();
        }

        OnLeave();
    }

    public virtual void RestorCanAttack() => basicAnimator.IsFinishAttack = false;

    private void OnLeave()
    {
        ResetCombo();
        comboNumber = 0;
        controller.SetUpState(nextState);
    }

    public virtual void ResetCombo()
    {
        comboNumber = 0;

        basicAnimator.IsFinishAttack = false;
        human_Basic_Controller.IsOnNotHitReact = false;
        human_Basic_Controller.SetIsAttack(false);
    }

    public override void StateLeave()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State NpcAttack ");

        ResetCombo();
        StopAllCoroutines();
        basicAnimator.SetOffOnRootMotion(false);
    }
}
