using UnityEngine;

public class Player_StateRotation : AbstractState
{
    [Header("Setting Player_StateRotation")]
    [SerializeField] private float speedFreeRotation = 10;
    [SerializeField] private float speedLockRotation = 3;
    [SerializeField] private bool onlyOnTarget;

    private Player_Controller player_Controller;
    private CharacterController characterController;
    private Vector3 currentLookDirection;

    public override void StateEnter() 
    { 
        if (controller.CanSeeDebug) Debug.Log("Entrato in State Rotation"); 

        if(characterController == null) characterController = controller.GetComponent<CharacterController>();
        if(player_Controller == null) player_Controller = controller.GetComponent<Player_Controller>();
    }

    public override void StateUpdate() => RotationCharacter();

    private void RotationCharacter()
    {
        if (!player_Controller.HasTarget) FreeRotation();
        else LockRotation();
    }

    private void FreeRotation()
    {
        if(onlyOnTarget) return;

        currentLookDirection = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);

        if (currentLookDirection.sqrMagnitude < 0.01f) return;
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, Quaternion.LookRotation(currentLookDirection.normalized), speedFreeRotation * Time.deltaTime);
    }

    private void LockRotation()
    {
        Vector3 directionToTarget = player_Controller.GetTarget().position - transform.position;
        directionToTarget.y = 0;
        if (directionToTarget.sqrMagnitude < 0.01f) return;
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, Quaternion.LookRotation(directionToTarget.normalized), speedLockRotation * Time.deltaTime);
    }

    public override void StateLeave() { if (controller.CanSeeDebug) Debug.Log("Uscito dallo State Rotation"); }
}
