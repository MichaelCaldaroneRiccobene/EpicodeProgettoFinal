using System;
using UnityEngine;

public class LifeController : MonoBehaviour, I_Damageble
{
    [Header("Setting Life")]
    [SerializeField] int maxLife = 100;

    private int life;

    public Action<int, Vector3> FisicalDamage;

    private void Start() => life = maxLife;

    public void OnUpdateLife(int value)
    {
        int hp = life;
        hp = Mathf.Clamp(hp + value,0,maxLife);

        Debug.Log("HP " + hp + gameObject.name,transform);
        if (hp < life)
        {
            life = hp;
        }
        else if (hp > life)
        {
            life = hp;
            // Heal
        }

        if (hp <= 0)
        {
            life = 0;
            Debug.Log("I am Death " + gameObject.name + transform);
        }
    }

    public void OnFisicalDamage(int value, Vector3 hitPoint) => FisicalDamage?.Invoke(value, hitPoint);
}
