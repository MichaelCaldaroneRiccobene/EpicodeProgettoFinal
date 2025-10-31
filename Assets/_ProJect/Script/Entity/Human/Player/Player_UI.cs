using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : GenericSingleton<Player_UI>    
{
    [SerializeField] private float delayUpdateYellowLife = 0.5f;
    [SerializeField] private float speedUpdateYellowLife = 0.4f;

    [SerializeField] private GameObject panelUI;

    [SerializeField] private Slider sliderLife;
    [SerializeField] private Slider sliderLifeYellow;

    [SerializeField] private Slider sliderStamina;
    [SerializeField] private Slider sliderStaminaYellow;

    private bool isUpdateYellowLife;
    private bool isUpdateYellowStamina;

    private void Update()
    {
        if (isUpdateYellowLife)
        {
            if (sliderLifeYellow.value > sliderLife.value) sliderLifeYellow.value -= speedUpdateYellowLife * Time.deltaTime;
            else isUpdateYellowLife = false;
        }

        if(isUpdateYellowStamina)
        {
            if (sliderStaminaYellow.value > sliderStamina.value)
            {
                sliderStaminaYellow.value -= speedUpdateYellowLife * Time.deltaTime;
            }
            else if(sliderStaminaYellow.value < sliderStamina.value)
            {
                sliderStaminaYellow.value = sliderStamina.value;
            }
            else if(sliderStaminaYellow.value == sliderStamina.value)
            {
                isUpdateYellowStamina = false;
            }
        }
    }

    public void UpdateLife(int life, int maxLife)
    {
        sliderLife.value = (float)life / maxLife;
        isUpdateYellowLife = false;
        if (!isUpdateYellowLife) { Utility.DelayAction(this, delayUpdateYellowLife, () => { isUpdateYellowLife = true; }); }
    }

    public void UpdateStamina(int stamina, int maxStamina)
    {
        sliderStamina.value = (float)stamina / maxStamina;
        isUpdateYellowStamina = false;
        if (!isUpdateYellowStamina) { Utility.DelayAction(this, delayUpdateYellowLife, () => { isUpdateYellowStamina = true; }); }
    }

    public void ShowUIOrHide(bool value) => panelUI.SetActive(value);
}
