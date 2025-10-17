using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Setting Weapon Basic ")]
    [SerializeField] private AttackList_SO attacksList;
    [SerializeField] private int costStaminaAttackBase = 30;
    [SerializeField] private float timeForCallCombo = 0.5f;

    public Transform OwnerWepon { get; set; }
    public AttackList_SO AttacksList => attacksList;
    public float TimeForCallCombo => timeForCallCombo;
    public int CostStaminaAttackBase => costStaminaAttackBase;


    public int DamageWepon { get; set; }
}
