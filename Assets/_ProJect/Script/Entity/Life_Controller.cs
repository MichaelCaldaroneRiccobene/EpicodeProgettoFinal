using System;
using UnityEngine;

public class LifeController : MonoBehaviour, I_Damageble
{
    [Header("Setting Life")]
    [SerializeField] int maxLife = 100;

    private int life;

    public Action<int,int> UpdateLifeHud;
    public Action<int, Vector3> FisicalDamage;

    private void Start() => life = maxLife;

    public void OnUpdateLife(int value)
    {
        int currentLife = life;
        currentLife = Mathf.Clamp(currentLife + value,0,maxLife);

        if (currentLife < life)
        {
            life = currentLife;
        }
        else if (currentLife > life)
        {
            life = currentLife;
            // Heal
        }

        if (currentLife <= 0)
        {
            life = 0;
            Debug.Log("I am Death " + gameObject.name + transform);
        }

        UpdateLifeHud?.Invoke(life,maxLife);
    }


    public bool IsDead() => life <= 0;
    public void OnFisicalDamage(int value, Vector3 hitPoint) => FisicalDamage?.Invoke(value, hitPoint);
}
