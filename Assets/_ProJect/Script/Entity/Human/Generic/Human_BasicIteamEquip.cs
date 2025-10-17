using UnityEngine;

public class Human_BasicIteamEquip : MonoBehaviour
{
    [Header("Setting ")]
    [SerializeField] private Weapon weapon;

    [SerializeField] private Transform positionArmRight;
    [SerializeField] private Transform positionArmLeft;
    [SerializeField] private Transform positionShield;

    private SetUpPosizioneAndRoationItem[] posRotItems;

    public Weapon Weapon => weapon;

    private void Awake()
    {
        weapon.OwnerWepon = transform;
        posRotItems = GetComponentsInChildren<SetUpPosizioneAndRoationItem>();

        if (posRotItems.Length <= 0) return;
        foreach(SetUpPosizioneAndRoationItem set in  posRotItems )
        {
            set.PositionArmLeft = positionArmLeft;
            set.PositionArmRight = positionArmRight;
            set.PositionShield = positionShield;
        }
    }
}
