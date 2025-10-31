using System;
using UnityEngine;

public class LifeController : MonoBehaviour, I_Damageble
{
    [Header("Setting Life")]
    [SerializeField] int maxLife = 100;
    [SerializeField] private ObjTypePoolling vfxBlood = ObjTypePoolling.HitBlood;
    [SerializeField] private AudioList_SO hitSound;

    private int life;
    private Vector3 saveHitPoint;
    private bool stopDamage;

    public Action<int,int> UpdateLifeHud;
    public Action<int, Vector3> FisicalDamage;

    private void Start()
    {
        UpdateLifeHud?.Invoke(life, maxLife);
    }

    private void OnEnable()
    {
        life = maxLife;
        UpdateLifeHud?.Invoke(life, maxLife);
    }

    public void OnUpdateLife(int value)
    {
        if(stopDamage) return;

        stopDamage = true;

        Utility.DelayAction(this, 0.5f, () => { stopDamage = false; });
        int currentLife = life;
        currentLife = Mathf.Clamp(currentLife + value,0,maxLife);

        if (currentLife < life)
        {
            life = currentLife;
            if (saveHitPoint != Vector3.zero) ManagerPooling.Instance.GetObjFromPool(vfxBlood, saveHitPoint, Quaternion.identity);

            Debug.Log("Physical Damage NEW: " + value, transform);
            hitSound.PlaySound(transform);
        }
        else if (currentLife > life)
        {
            life = currentLife;
            // Heal
        }

        if (currentLife <= 0)
        {
            life = 0;
        }

        
        UpdateLifeHud?.Invoke(life,maxLife);
    }

    public void RestorHp()
    {
        life = maxLife;
        UpdateLifeHud?.Invoke(life, maxLife);
    }

    public bool IsDead() => life <= 0;
    public void OnPhysicalDamage(int value, Vector3 hitAttaker, Vector3 hitPoint)
    {
        saveHitPoint = hitPoint;
        FisicalDamage?.Invoke(value, hitAttaker);

        Debug.Log("Physical Damage: " + value,transform);
    }
}
