using UnityEngine;

public enum PositionWeapon { ArmRight, ArmLeft, Shield }
public class SetUpPosizioneAndRoationItem : MonoBehaviour
{
    [Header("Setting Position And Rotation Weapon")]

    [SerializeField] private PositionWeapon positionWeapon;
    [SerializeField] private Vector3 localPosition;
    [SerializeField] private Vector3 localRotation;

    public Transform PositionArmRight;
    public Transform PositionArmLeft;
    public Transform PositionShield;

    private void Start()
    {
        switch (positionWeapon)
        {
            case PositionWeapon.ArmRight:
                transform.parent = PositionArmRight;
                break;
            case PositionWeapon.ArmLeft:
                transform.parent = PositionArmLeft;
                break;
            case PositionWeapon.Shield:
                transform.parent = PositionShield;
                break;
        }

        transform.localPosition = localPosition;
        transform.localRotation = Quaternion.Euler(localRotation);
    }
}
