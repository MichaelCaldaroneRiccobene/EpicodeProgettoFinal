using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectAttack { AttackBase,AttackSpecial}
public class State_BossAttack : AbstractState
{
    [Header("Setting BossAttack")]
    [SerializeField] protected AbstractState nextState;
    [SerializeField] protected SelectAttack typeAttack;

    protected Npc_Controller npc_Controller;
    protected Human_BasicAnimator basicAnimator;
    protected Human_BasicIteamEquip iteamEquip;

    protected Stamina_Controller stamina_Controller;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State BossAttack ");

        if (npc_Controller == null) npc_Controller = controller.GetComponent<Npc_Controller>();

        if (basicAnimator == null) basicAnimator = controller.GetComponentInChildren<Human_BasicAnimator>();
        if (iteamEquip == null) iteamEquip = controller.GetComponent<Human_BasicIteamEquip>();
        if (stamina_Controller == null) stamina_Controller = controller.GetComponent<Stamina_Controller>();

        SelectModeNpc();
        ResetAttack();
    }

    private void SelectModeNpc()
    {
        switch (typeAttack)
        {
            case SelectAttack.AttackBase:
                StartCoroutine(AttackRoutine(iteamEquip.Weapon.AttacksList,Random.Range(0, iteamEquip.Weapon.AttacksList.AttackParameters.Count)));
                break;
            case SelectAttack.AttackSpecial:
                StartCoroutine(AttackRoutine(iteamEquip.Weapon.AttacksListSpecial,0));
                break;
        }
    }

    private void ProcessAttack(AttackList_SO typeAttackList,int numberAttack)
    {
        if (npc_Controller.IsOnBlockMode) return;

        npc_Controller.IsOnNotHitReact = false;

        basicAnimator.SetOffOnRootMotion(true);
        basicAnimator.OnAttackMeeleName(typeAttackList.AttackParameters[numberAttack].AnimatorOverrideController);
                                        
        iteamEquip.Weapon.DamageWepon = typeAttackList.AttackParameters[numberAttack].Damage;

        npc_Controller.IsOnNotHitReact = true;
        npc_Controller.EnableDisableInStoppableAttack(true);
    }

    private IEnumerator AttackRoutine(AttackList_SO typeAttackList, int numberAttack)
    {
        yield return null;

        if (!stamina_Controller.CheckCanUseStamina(iteamEquip.Weapon.CostStaminaAttackBase))
        {
            OnLeave();
            yield break;
        }

        yield return null;

        ProcessAttack(typeAttackList, numberAttack);

        yield return null;

        npc_Controller.SetIsAttack(true);
        while (!basicAnimator.IsFinishAttack) yield return null;
        stamina_Controller.OnUpdateStamina(-iteamEquip.Weapon.CostStaminaAttackBase);
        npc_Controller.SetIsAttack(false);
        RestorCanAttack();

        OnLeave();
    }

    public virtual void RestorCanAttack()
    {
        basicAnimator.IsFinishAttack = false;
        npc_Controller.EnableDisableInStoppableAttack(false);
    }

    private void OnLeave()
    {
        ResetAttack();
        StopAllCoroutines();
        basicAnimator.SetOffOnRootMotion(false);
        controller.SetUpState(nextState);
    }

    public virtual void ResetAttack()
    {

        basicAnimator.IsFinishAttack = false;
        npc_Controller.IsOnNotHitReact = false;
        npc_Controller.SetIsAttack(false);
        npc_Controller.EnableDisableInStoppableAttack(false);
    }

    public override void StateLeave()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State BossAttack ");

        ResetAttack();
        StopAllCoroutines();
        basicAnimator.SetOffOnRootMotion(false);
    }
}
