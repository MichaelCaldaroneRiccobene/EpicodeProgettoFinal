using System.Collections;
using UnityEngine;

public class Player_StateAttackBase : AbstractState
{
    [Header("Setting StateAttack")]
    [SerializeField] protected AbstractState nextState;

    protected Human_Basic_Controller human_Basic_Controller;
    protected Human_BasicInput basicInput;
    protected Human_BasicAnimator basicAnimator;
    protected Human_BasicIteamEquip iteamEquip;

    protected Stamina_Controller stamina_Controller;

    protected Coroutine comboCoroutine;
    protected AnimatorOverrideController animatorOverrideController;

    protected bool isOnAttack;
    protected bool isOnCombo;
    protected int comboNumber;

    protected float timerCombo;

    protected bool onNonStamina;

    public override void StateEnter() 
    { 
        if (controller.CanSeeDebug) Debug.Log("Entrato in State Attack ");

        if(human_Basic_Controller == null) human_Basic_Controller = controller.GetComponent<Human_Basic_Controller>();
        if (basicInput == null)
        {
            basicInput = controller.GetComponent<Human_BasicInput>();
            basicInput.OnAttack += OnAttack;
        }

        if(basicAnimator == null) basicAnimator = controller.GetComponentInChildren<Human_BasicAnimator>();
        if(iteamEquip == null) iteamEquip = controller.GetComponent<Human_BasicIteamEquip>();
        if(stamina_Controller ==  null) stamina_Controller = controller.GetComponent<Stamina_Controller>();

        ResetCombo();
        OnAttack();
    }

    public override void StateUpdate() { TimerCombo(); }

    private void TimerCombo()
    {
        if(isOnCombo || !isOnAttack)
        {
            if(!stamina_Controller.CheckCanUseStamina(iteamEquip.Weapon.AttacksList.AttackParameters[comboNumber].CostStamina))
            {
                onNonStamina = true;
                isOnCombo = false;
                comboNumber = 0;
                controller.SetUpState(nextState);
            }
        }

        if (isOnCombo && !isOnAttack && (timerCombo -= Time.deltaTime) <= 0f)
        {
            isOnCombo = false;
            comboNumber = 0;
            controller.SetUpState(nextState);
        }
    }

    public virtual void OnAttack()
    {
        if (!stamina_Controller.CheckCanUseStamina(iteamEquip.Weapon.CostStaminaAttackBase) && !isOnAttack)
        {
            if (onNonStamina) return;

            onNonStamina = true;
        }
        else
        {
            if (isOnAttack) return;
            isOnAttack = true;
            human_Basic_Controller.IsOnNotHitReact = false;
   
            basicAnimator.SetOffOnRootMotion(true);
            basicAnimator.OnAttackMeeleName(iteamEquip.Weapon.AttacksList.AttackParameters[comboNumber].AnimatorOverrideController);

            iteamEquip.Weapon.DamageWepon = iteamEquip.Weapon.AttacksList.AttackParameters[comboNumber].Damage;
            timerCombo = iteamEquip.Weapon.TimeForCallCombo;

            comboNumber++;
            if (comboNumber >= iteamEquip.Weapon.AttacksList.AttackParameters.Count)
            {
                human_Basic_Controller.IsOnNotHitReact = true;
                comboNumber = 0;
            }

            if (comboCoroutine != null) StopCoroutine(comboCoroutine);
            comboCoroutine = StartCoroutine(RestorAttackRoutine());
        }
    }

    private IEnumerator RestorAttackRoutine()
    {
        yield return null;

        human_Basic_Controller.SetIsAttack(true);
        while (!basicAnimator.IsFinishAttack) yield return null;
        stamina_Controller.OnUpdateStamina(-iteamEquip.Weapon.CostStaminaAttackBase);
        human_Basic_Controller.SetIsAttack(false);
        RestorCanAttack();
    }

    public virtual void RestorCanAttack()
    {
        isOnCombo = true;
        isOnAttack = false;
        basicAnimator.IsFinishAttack = false;

        timerCombo = iteamEquip.Weapon.TimeForCallCombo;
    }

    public virtual void ResetCombo()
    {
        comboNumber = 0;

        isOnCombo = false;
        isOnAttack = false;
        onNonStamina = false;

        basicAnimator.IsFinishAttack = false;
        human_Basic_Controller.IsOnNotHitReact = false;
        human_Basic_Controller.SetIsAttack(false);

        timerCombo = iteamEquip.Weapon.TimeForCallCombo;
    }

    public override void StateLeave() 
    { 
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State Attack ");

        ResetCombo();
        if(comboCoroutine != null) { StopCoroutine(comboCoroutine); comboCoroutine = null; }
        basicAnimator.SetOffOnRootMotion(false);
    }

    public virtual void OnDisable()
    {
       if(basicInput != null) basicInput.OnAttack -= OnAttack;

        basicInput = null;
    }
}
