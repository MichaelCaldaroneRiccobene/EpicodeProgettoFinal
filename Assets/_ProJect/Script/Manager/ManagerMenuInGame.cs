using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerMenuInGame : ManagerMenu
{

    public override void Start()
    {
        foreach (ButtonList buttonList in buttonList)
        {
            foreach (Button button in buttonList.buttons) button.interactable = true;
        }
    }


    public override void EnableDisableMenu()
    {
        EnablePannelAll = !EnablePannelAll;

        if (EnablePannelAll)
        {
            pannelAll.SetActive(EnablePannelAll);
            SetUpPannel(TypeUI.ReturnMenu);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
       else Resume();
    }

    public override void Resume()
    {
        EnablePannelAll = false;

        pannelAll.SetActive(EnablePannelAll);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetUpPannel(TypeUI.None);
    }
}
