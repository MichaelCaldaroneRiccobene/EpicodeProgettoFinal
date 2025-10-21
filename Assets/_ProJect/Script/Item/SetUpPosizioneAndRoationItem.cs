using UnityEngine;

public enum TipeItem { ArmRight, ArmLeft, Shield }
public class SetUpPosizioneAndRoationItem : MonoBehaviour
{
    [Header("Setting Position And Rotation Weapon")]

    [SerializeField] private TipeItem tipeItem;
    [SerializeField] private Vector3 localPosition;
    [SerializeField] private Vector3 localRotation;

    public Transform PositionArmRight;
    public Transform PositionArmLeft;
    public Transform PositionShield;

    private void Start()
    {
        switch (tipeItem)
        {
            case TipeItem.ArmRight:
                transform.parent = PositionArmRight;
                break;
            case TipeItem.ArmLeft:
                transform.parent = PositionArmLeft;
                break;
            case TipeItem.Shield:
                transform.parent = PositionShield;
                break;
        }

        transform.localPosition = localPosition;
        transform.localRotation = Quaternion.Euler(localRotation);
    }
}
