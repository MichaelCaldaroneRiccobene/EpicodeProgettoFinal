using DG.Tweening;
using System;
using UnityEngine;

public class Human_BasicAnimator : MonoBehaviour
{
    [Header("Setting Time For Animation")]
    [SerializeField] protected float timeToRestorPosRot = 0.25f;
    [SerializeField] protected float transitionDuration = 0.15f;
    [SerializeField] protected float smoothAnimation = 0.1f;

    [Header("Setting Movement")]
    [SerializeField] protected string parameterFloatSpeed = "Speed";
    [SerializeField] protected string parameterFloatDirection = "Direction";

    [Header("Parameter Hit Damage")]
    [SerializeField] protected string parameterTriggerHit = "Hit";
    [SerializeField] protected string parameterTriggerOnIdle = "Idle";

    [Header("Parameter Combat")]
    [SerializeField] protected string parameterTriggerAttack = "Roll";
    [SerializeField] protected string parameterTriggerOnRoll = "OnRoll";

    [Header("Parameter Shield")]
    [SerializeField] protected string parameterBoolOnShield = "OnShield";
    [SerializeField] protected string parameterTriggerShildReact = "ShildReact";

    protected Animator animator;
    protected Tween restorPosRotTween;

    public bool IsFinishAttack {  get; set; }

    public Action<bool> OnEnableDisableDoDamage;

    public virtual void Awake() => animator = GetComponentInChildren<Animator>();

    #region AnimationMoving
    public virtual void AnimationMoving(float currentSpeed, float maxSpeed) { }
    #endregion

    #region LogicAttack
    public virtual void OnAttackMeeleName(AnimatorOverrideController animOver)
    {
        animator.runtimeAnimatorController = animOver;
        SelectAnimation(false, parameterTriggerAttack);
    }

    public virtual bool OnFinishAttack() => IsFinishAttack = true;

    public virtual void StartDoDamage() => OnEnableDisableDoDamage?.Invoke(true);

    public virtual void StopDoDamage() => OnEnableDisableDoDamage?.Invoke(false);

    #endregion

    #region LogicHit
    public virtual void OnHitReact()
    {
        SelectAnimation(true, parameterTriggerHit);
        StopDoDamage();
    }
    #endregion

    #region LogicShield
    public virtual void OnShieldIdle(bool value) => animator.SetBool(parameterBoolOnShield, value);

    public virtual void OnShieldReact() => SelectAnimation(false, parameterTriggerShildReact);

    #endregion

    #region LogicRoll
    public virtual void OnRoll() => SelectAnimation(false, parameterTriggerOnRoll);
    #endregion

    #region Setting General

    public virtual void SelectAnimation(bool isSmooth, string nameAnimation)
    {
        if (isSmooth) animator.CrossFade(nameAnimation, transitionDuration, 0, 0);
        else animator.Play(nameAnimation, 0, 0f);
    }

    public virtual void SetOffOnRootMotion(bool value)
    {
        if (animator == null) return;

        animator.applyRootMotion = value;

        if (restorPosRotTween != null) restorPosRotTween.Kill();
        restorPosRotTween = transform.DOLocalRotate(Vector3.zero, timeToRestorPosRot).SetEase(Ease.OutQuad);

        StopDoDamage();
    }
    public virtual void ReturnIdle()
    {
        SelectAnimation(true, parameterTriggerOnIdle);
        StopDoDamage();
    }


    public virtual void OnAnimatorMove() { if (animator.applyRootMotion) OnMoveWithRootMotion(); }

    public virtual void OnMoveWithRootMotion() { }

    #endregion
}
