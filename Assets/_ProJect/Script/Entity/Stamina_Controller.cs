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

    private int stamina;
    private bool onRecoverStamina;

    public int Stamina => stamina;

    public Action<int, int> UpdateStaminaHud;

    private void Start()
    {
        stamina = maxStamina;
        waitForSecondsStaminaRecover = new WaitForSeconds(timeForRecoverStamina);
    }

    public void OnUpdateStamina(int value)
    {
        int currentStamina = stamina;
        currentStamina = Mathf.Clamp(currentStamina + value, 0, maxStamina);

        if (currentStamina < stamina)
        {
            stamina = currentStamina;

            if (!onRecoverStamina) coroutineStaminaRecover = StartCoroutine(RecoverStaminaRoutine());
        }
        else if (currentStamina > stamina)
        {
            stamina = currentStamina;
        }

        UpdateStaminaHud?.Invoke(stamina,maxStamina);
    }

    public bool CheckCanUseStamina(int value) => value <= stamina;

    public float GetPercentStamina() => (float)stamina/maxStamina;

    private IEnumerator RecoverStaminaRoutine()
    {
        onRecoverStamina = true;
        while(stamina < maxStamina)
        {
            yield return waitForSecondsStaminaRecover;
            OnUpdateStamina(recoverStaminaOnTimeStamina);
        }
        onRecoverStamina = false;
    }
}
