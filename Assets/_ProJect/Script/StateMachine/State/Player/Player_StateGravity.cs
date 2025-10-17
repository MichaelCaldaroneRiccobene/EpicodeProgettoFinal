using UnityEngine;

public class Player_StateGravity : AbstractState
{
    [Header("Setting Player_StateGravity ")]
    [SerializeField] private float gravity = 10;

    private CharacterController characterController;

    private Vector3 direction;
    private float verticalSpeed;

    public override void StateEnter() 
    { 
        if (controller.CanSeeDebug) Debug.Log("Entrato in State Gravity "); 

        if(characterController == null) characterController = controller.GetComponent<CharacterController>();
        direction = Vector3.zero;
    }

    public override void StateUpdate() 
    {
        direction.y = VerticalForceGravity();
        characterController.Move(direction * (gravity * Time.deltaTime));
    }

    private float VerticalForceGravity()
    {
        if (characterController.isGrounded) verticalSpeed = -1;
        else verticalSpeed -= gravity * Time.deltaTime;
        return verticalSpeed;
    }

    public override void StateLeave() { if (controller.CanSeeDebug) Debug.Log("Uscito dallo State Gravity "); }
}
