using UnityEngine;

public class Player_Controller : Human_Basic_Controller
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
