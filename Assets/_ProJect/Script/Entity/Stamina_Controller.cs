using System;
using System.Collections;
using UnityEngine;

public class Stamina_Controller : MonoBehaviour
{
    [SerializeField] private int maxStamina = 150;
    [SerializeField] private int recoverStaminaOnTimeStamina = 5;

    [SerializeField] private float timeForRecoverStamina = 0.5f;

    private Coroutine coroutineStaminaRecover;
    private WaitForSeconds waitForSecondsStaminaRecover;    

    public int stamina;
    public bool onRecoverStamina;

    public int Stamina => stamina;


    private void Start()
    {
        stamina = maxStamina;
        waitForSecondsStaminaRecover = new WaitForSeconds(timeForRecoverStamina);
    }

    public void OnUpdateStamina(int value)
    {
        int currentStamina = stamina;
        currentStamina = Mathf.Clamp(currentStamina + value, 0, maxStamina);

        //Debug.Log("Stamina " + currentStamina + gameObject.name, transform);
        if (currentStamina < stamina)
        {
            stamina = currentStamina;

            if (!onRecoverStamina) coroutineStaminaRecover = StartCoroutine(RecoverStaminaRoutine());
        }
        else if (currentStamina > stamina)
        {
            stamina = currentStamina;
        }

        if (currentStamina <= 0)
        {
            stamina = 0;
        }
    }

    public bool CheckCanUseStamina(int value) => value <= stamina;

    public void UseStamina(int value,Action action)
    {
        int currentStamina = stamina;
        currentStamina = Mathf.Clamp(currentStamina + value, 0, maxStamina);

        //Debug.Log("Stamina " + currentStamina + gameObject.name, transform);
        if (currentStamina < stamina)
        {
            stamina = currentStamina;
            action();
            if (!onRecoverStamina) coroutineStaminaRecover = StartCoroutine(RecoverStaminaRoutine());
        }
        else if (currentStamina > stamina)
        {
            stamina = currentStamina;
        }

        if (currentStamina <= 0)
        {
            stamina = 0;
        }   
    }

    private IEnumerator RecoverStaminaRoutine()
    {
        while(stamina < maxStamina)
        {
            yield return waitForSecondsStaminaRecover;
            OnUpdateStamina(recoverStaminaOnTimeStamina);
        }
    }
}
