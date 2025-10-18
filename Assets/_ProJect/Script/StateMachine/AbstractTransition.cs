using System.Collections;
using UnityEngine;

public abstract class AbstractTransition : MonoBehaviour
{
    [SerializeField] private AbstractState nextState;

    [SerializeField] protected float timeCheckRoutine = 0.1f;
    [SerializeField] protected bool isOnUpdate;

    protected FSM_Controller controller;
    protected Coroutine coroutineCheckTime;
    private WaitForSeconds waitForCheckTime;
    protected bool conditionMet;

    public AbstractState NextState => nextState;

    public virtual void SetUp(FSM_Controller controller)
    {
        this.controller = controller;
        waitForCheckTime = new WaitForSeconds(timeCheckRoutine);
    }
    public abstract void CheckConditionUpdate();

    public virtual bool IsConditionMet()
    {
        if (!isOnUpdate) return conditionMet;
        else
        {
            CheckConditionUpdate();
            return conditionMet;
        }
    }

    public virtual IEnumerator CheckTimeRoutine()
    {
        while (!conditionMet)
        {
            yield return waitForCheckTime;
            CheckConditionUpdate();
        }
    }

    public virtual void OnEnable()
    {
        conditionMet = false;
        if (!isOnUpdate && controller)
        {
            if (coroutineCheckTime != null) StopCoroutine(coroutineCheckTime);
            coroutineCheckTime = StartCoroutine(CheckTimeRoutine());
        }
    }

    public virtual void OnDisable()
    {
        conditionMet = false;
        if(coroutineCheckTime != null) { StopCoroutine(coroutineCheckTime);coroutineCheckTime = null; }
    }
}
