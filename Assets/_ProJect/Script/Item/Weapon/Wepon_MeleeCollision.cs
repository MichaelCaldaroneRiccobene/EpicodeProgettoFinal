using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wepon_MeleeCollision : Weapon
{
    [Header("Setting WeaponMeleeCollision ")]
    [SerializeField] private Transform[] pointsWepon;
    [SerializeField] private bool friendlyFire;
    [SerializeField] private EffectScreen_SO effectScreen;
    [SerializeField] private AudioClip swordClip;

    private List<Transform> possibleTarget = new List<Transform>();

    private Player_Controller playerController;
    private Human_Basic_Controller human_Basic_Controller;
    private Human_BasicAnimator basicAnimator;
    private Vector3 savePosition;
    private bool canDoDamage;

    public virtual void Start()
    {
        human_Basic_Controller = OwnerWepon.GetComponent<Human_Basic_Controller>();
        basicAnimator = OwnerWepon.GetComponentInChildren<Human_BasicAnimator>();
        playerController = OwnerWepon.GetComponent<Player_Controller>();    

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
        if (ManagerPooling.Instance) ManagerPooling.Instance.GetAudioFromPool(ObjTypePoolling.Audio, OwnerWepon.transform.position, Quaternion.identity, swordClip, 1, 1.1f, 1.3f, true, true);

        for (int i = 0; i < pointsWepon.Length - 1; i++)
        {
            if (Physics.Linecast(pointsWepon[i].position, pointsWepon[i + 1].position, out RaycastHit hit))
            {
                if (hit.transform.transform == OwnerWepon || possibleTarget.Contains(hit.transform)) continue;
                if(hit.transform.TryGetComponent(out I_Team team)) { if (team.GetTeam() == human_Basic_Controller.GetTeam() && !friendlyFire) continue; }


                if (hit.transform.TryGetComponent(out I_Damageble i_Damageble))
                {
                    i_Damageble.OnPhysicalDamage(-DamageWepon, savePosition,hit.point);
                    possibleTarget.Add(hit.transform);

                    if (playerController)
                    {
                        effectScreen.ShockEffect(playerController);
                        effectScreen.ShakeCamera(playerController.transform.position);
                    }
                    
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
