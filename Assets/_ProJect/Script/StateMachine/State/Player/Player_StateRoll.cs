using System.Collections;
using UnityEngine;

public class Player_StateRoll : AbstractState
{
    [Header("Setting Player_StateRoll")]
    [SerializeField] private AbstractState nextState;
    [SerializeField] private float duration = 0.75f;

    private Player_Controller player_Controller;
    private Player_Animator player_Animator;
    private Player_Input player_Input;

    private Transform currentTarget;

    private Vector3 direction;
    private float horizontalInput;
    private float verticalInput;

    public override void StateEnter()
    { 
        if (controller.CanSeeDebug) Debug.Log("Entrato in State Roll"); 
        SetStart();
    }

    private void SetStart()
    {

        if (player_Controller == null) player_Controller = controller.GetComponent<Player_Controller>();

        if (player_Input == null)
        {
            player_Input = controller.GetComponent<Player_Input>();
            player_Input.OnHorizontalAndVerticalInput += SetHorizontalAndVerticalInput;
        }

        if (player_Animator == null) player_Animator = controller.GetComponentInChildren<Player_Animator>();
        currentTarget = null;
        StartCoroutine(OnRollRoutine());
    }

    private void SetHorizontalAndVerticalInput(float horizontal, float vertical) { horizontalInput = horizontal; verticalInput = vertical; }

    private IEnumerator OnRollRoutine()
    {
        if(player_Controller.HasTarget) currentTarget = player_Controller.GetTarget();
        player_Controller.IsonRollMode = true;
        player_Controller.SetTarget(null);

        yield return null;
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        direction = forward * verticalInput + right * horizontalInput;
        direction.y = 0;

        Vector3 rollDirection = direction.sqrMagnitude > 0.01f ? direction.normalized : controller.transform.forward;
        controller.transform.rotation = Quaternion.LookRotation(rollDirection);

        yield return null;
        player_Animator.SetOffOnRootMotion(true);
        yield return null;
        player_Animator.OnRoll();

        yield return new WaitForSeconds(duration);
        player_Animator.SetOffOnRootMotion(false);
        if (currentTarget != null) player_Controller.SetTarget(currentTarget);
        currentTarget = null;

        player_Controller.IsonRollMode = false;
        controller.SetUpState(nextState);
    }

    public override void StateLeave()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State Roll");
        StopAllCoroutines();
        player_Controller.IsonRollMode = false;
    }

    private void OnDisable()
    {
        if(player_Input != null) player_Input.OnHorizontalAndVerticalInput -= SetHorizontalAndVerticalInput;

        player_Input = null;
    }
}
