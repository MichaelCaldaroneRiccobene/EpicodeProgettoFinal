using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wepon_MeleeCollision : Weapon
{
    [Header("Setting WeaponMeleeCollision ")]
    [SerializeField] private Transform[] pointsWepon;
    [SerializeField] private bool friendlyFire;
    [SerializeField] private EffectScreen_SO effectScreen;
    [SerializeField] private AudioList_SO swingAudio;

    private List<Transform> possibleTarget = new List<Transform>();
    private List<Transform> saveTempTransformHit = new List<Transform>();

    private Player_Controller playerController;
    private Human_Basic_Controller human_Basic_Controller;
    private Human_BasicAnimator basicAnimator;
    private Vector3 savePosition;

    private bool canDoAudioFirstAudio;
    private bool canDoAudio;
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

        if (!canDoAudioFirstAudio) swingAudio.PlaySound(OwnerWepon);
        canDoAudioFirstAudio = true;

        for (int i = 0; i < pointsWepon.Length - 1; i++)
        {
            canDoAudio = false;

            if (Physics.Linecast(pointsWepon[i].position, pointsWepon[i + 1].position, out RaycastHit hit))
            {
                if (hit.transform.transform == OwnerWepon || possibleTarget.Contains(hit.transform) || saveTempTransformHit.Contains(hit.transform)) continue;
                if(hit.transform.TryGetComponent(out I_Team team)) { if (team.GetTeam() == human_Basic_Controller.GetTeam() && !friendlyFire) continue; }

                Debug.Log(hit.transform);
                saveTempTransformHit.Add(hit.transform);

                if (!canDoAudio)
                {
                    swingAudio.PlaySound(OwnerWepon);
                    canDoAudio = true;
                }

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
        if (!this.canDoDamage)
        {
            possibleTarget.Clear();
            saveTempTransformHit.Clear();
        }
        this.canDoDamage = canDoDamage;
        canDoAudio = canDoDamage;
        canDoAudioFirstAudio = !canDoDamage;

        if (canDoDamage) savePosition = OwnerWepon.transform.position;
    }

    public void OnDisable()
    {
        if (basicAnimator != null)
        {
            basicAnimator.OnEnableDisableDoDamage -= OnCanDoDamage;
        }
    }
}
