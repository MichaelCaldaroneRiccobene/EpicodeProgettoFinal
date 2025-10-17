using UnityEngine;

public abstract class AbstractState : MonoBehaviour
{
    protected FSM_Controller controller;
    protected AbstractTransition[] transitions;

    public AbstractTransition[] Transitions => transitions;

    public abstract void StateEnter();
    public abstract void StateLeave();
    public virtual void StateUpdate() { }

    public virtual void SetUp(FSM_Controller controller)
    {
        this.controller = controller;
        transitions = GetComponents<AbstractTransition>();
    }

    public AbstractState EvaluateTransition()
    {
        foreach(AbstractTransition transition in transitions) if (transition.IsConditionMet()) return transition.NextState;
        return null;
    }
}
