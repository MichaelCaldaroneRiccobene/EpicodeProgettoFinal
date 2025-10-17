using UnityEngine;

public class Player_Controller : Human_Basic_Controller
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void UpdateLifeHud(int life, int maxLife) => Player_UI.Instance.UpdateLife(life, maxLife);

    public override void UpdateStaminaHud(int stamina, int maxStamina) => Player_UI.Instance.UpdateStamina(stamina, maxStamina);

}
