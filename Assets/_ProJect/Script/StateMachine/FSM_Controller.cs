using UnityEngine;
public class FSM_Controller : MonoBehaviour
{
    [Header("Setting FSM_Controller")]
    [SerializeField] protected AbstractState startState;

    [Header("Debug")]
    [SerializeField] protected AbstractState currentState;
    [SerializeField] protected AbstractState[] subStates;
    [SerializeField] protected float currentStateTime;
    [SerializeField] protected bool canSeeDebug;

    protected AbstractState[] availableStates;
    protected AbstractTransition[] transitionsState;
    protected AbstractState nextState;

    public float CurrentStateTime => currentStateTime;
    public bool CanSeeDebug => canSeeDebug;

    public virtual void Awake()
    {
        availableStates = GetComponentsInChildren<AbstractState>();
        foreach(AbstractState state in availableStates) state.SetUp(this);

        transitionsState = GetComponentsInChildren<AbstractTransition>();
        foreach(AbstractTransition trans in transitionsState)
        {
            trans.SetUp(this);
            trans.enabled = false;
        }
        transitionsState = null;
    }

    private void OnEnable()
    {
        if(availableStates.Length == 0) { Debug.LogWarning("No States Available"); return; }

        if (startState != null) SetUpState(startState);
        else SetUpState(availableStates[0]);
    }

    private void Update()
    {
        if (currentState == null) return;

        currentStateTime += Time.deltaTime;
        foreach(AbstractState state in subStates) state.StateUpdate();

        nextState = currentState.EvaluateTransition();
        if(nextState != null) SetUpState(nextState);
    }

    public virtual void SetUpState(AbstractState state)
    {
        if(currentState != null)
        {
            foreach (AbstractState subState in subStates)
            {
                if (subState.gameObject.activeInHierarchy) subState.StateLeave();
            }

            if(transitionsState.Length > 0) foreach (AbstractTransition transition in transitionsState) transition.enabled = false;

            subStates = null;
            transitionsState = null;
        }

        if(state ==  null) { Debug.LogWarning("No State Found"); return; }

        currentStateTime = 0;

        currentState = state;
        subStates = currentState.GetComponentsInChildren<AbstractState>();

        foreach (AbstractState subState in subStates)
        {
            if (subState.gameObject.activeInHierarchy) { subState.StateEnter(); transitionsState = subState.Transitions; }
        }

        foreach (AbstractTransition transition in transitionsState) transition.enabled = true;
    }
}
