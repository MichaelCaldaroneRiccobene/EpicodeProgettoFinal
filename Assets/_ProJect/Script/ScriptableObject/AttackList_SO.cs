using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttacksList", menuName = "AttacksList/Melee")]
public class AttackList_SO : ScriptableObject
{
    public List<AttackParameters> AttackParameters;
}

[System.Serializable]
public class AttackParameters
{
    public AnimatorOverrideController AnimatorOverrideController;
    public int Damage;
    public int CostStamina;
}
