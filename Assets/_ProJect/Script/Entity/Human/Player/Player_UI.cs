using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : GenericSingleton<Player_UI>    
{
    [SerializeField] private Image imageLife;
    [SerializeField] private Image imageStamina;

    public void UpdateLife(int life,int maxLife) => imageLife.fillAmount = (float)life / maxLife;

    public void UpdateStamina(int stamina, int maxStamina) => imageStamina.fillAmount = (float)stamina / maxStamina;
}
