using UnityEngine;

public class Player_StateIdleMovement : AbstractState
{
    [Header("Control Speed")]
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float jogSpeed = 2.5f;

    private Player_Input player_Input;
    private CharacterController characterController;
    private Player_Animator player_Animator;

    private Vector3 direction;
    private float horizontalInput;
    private float verticalInput;

    private float speed;
    private float currentSpeed;

    private bool isWalking;

    public override void StateEnter() 
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State IdleMovement ");
        SetUp();

        isWalking = false;
        SetUpSpeedMovement();
    }

    public override void StateUpdate() => MovementCharacter();

    private void SetUp()
    {
        if (player_Input == null)
        {
            player_Input = controller.GetComponent<Player_Input>();
            player_Input.OnHorizontalAndVerticalInput += SetHorizontalAndVerticalInput;
            player_Input.OnWalk += OnWalk;
        }

        if(characterController == null) characterController = controller.GetComponent<CharacterController>();

        if(player_Animator == null) player_Animator = controller.GetComponentInChildren<Player_Animator>();

        player_Animator.SetOffOnRootMotion(false);
        player_Animator.ReturnIdle();
    }

    private void SetHorizontalAndVerticalInput(float horizontal, float vertical) { horizontalInput = horizontal; verticalInput = vertical; }

    private void MovementCharacter()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0; right.y = 0;
        forward.Normalize(); right.Normalize();

        direction = forward * verticalInput + right * horizontalInput;
        if (direction.magnitude > 1) direction.Normalize();

        characterController.Move(direction * (speed * Time.deltaTime));

        currentSpeed = isWalking ? walkSpeed : jogSpeed;
        player_Animator.AnimationMoving(currentSpeed, jogSpeed);
    }

    private void OnWalk()
    {
        isWalking = !isWalking;
        SetUpSpeedMovement();
    }

    private void SetUpSpeedMovement() => speed = isWalking ? walkSpeed : jogSpeed;

    public override void StateLeave() 
    { 
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State IdleMovement ");
        isWalking = false;
    }

    private void OnDisable()
    {
        if (player_Input != null)
        {
            player_Input.OnHorizontalAndVerticalInput -= SetHorizontalAndVerticalInput;
            player_Input.OnWalk -= OnWalk;
        }
    }
}
