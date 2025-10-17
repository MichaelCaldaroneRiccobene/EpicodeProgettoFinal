using System.Collections.Generic;
using UnityEngine;

public class Wepon_MeleeCollision : Weapon
{
    [Header("Setting WeaponMeleeCollision ")]
    [SerializeField] private Transform[] pointsWepon;

    private List<Transform> possibleTarget = new List<Transform>();

    private Human_BasicAnimator basicAnimator;
    private Vector3 savePosition;
    private bool canDoDamage;

    public virtual void Start()
    {
        basicAnimator = OwnerWepon.GetComponentInChildren<Human_BasicAnimator>();

        SetAction();
    }

    public void SetAction()
    {
        if (basicAnimator != null)
        {
            basicAnimator.OnEnableDisableDoDamage += OnCanDoDamage;
        }
    }

    public void Update()
    {
        if (!canDoDamage) return;

        for (int i = 0; i < pointsWepon.Length; i++)
        {
            if (i == pointsWepon.Length - 1) break;

            if (Physics.Linecast(pointsWepon[i].position, pointsWepon[i + 1].position, out RaycastHit hit))
            {
                if (hit.transform.transform == OwnerWepon || possibleTarget.Contains(hit.transform)) continue;

                if (hit.transform.TryGetComponent(out I_Damageble i_Damageble))
                {
                    i_Damageble.OnFisicalDamage(-DamageWepon, savePosition);
                    possibleTarget.Add(hit.transform);
                }
            }
        }
    }

    private void OnCanDoDamage(bool canDoDamage)
    {
        if (!this.canDoDamage) possibleTarget.Clear();
        this.canDoDamage = canDoDamage;

        if(canDoDamage) savePosition = OwnerWepon.transform.position;
    }

    public void OnDisable()
    {
        if (basicAnimator != null)
        {
            basicAnimator.OnEnableDisableDoDamage -= OnCanDoDamage;
        }
    }
}
