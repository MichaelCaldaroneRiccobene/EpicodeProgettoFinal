using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CheckSavePoint { EnterSavePoint, ExitSavePoint }    
public class Transition_PlayerIsOnOrOffSavePoint : AbstractTransition
{
    [SerializeField] CheckSavePoint checkSavePoint;

    Player_Controller player_Controller;

    public override void SetUp(FSM_Controller controller)
    {
        base.SetUp(controller);

        player_Controller = controller.GetComponent<Player_Controller>();
    }

    public override void CheckConditionUpdate()
    {
        if(checkSavePoint == CheckSavePoint.EnterSavePoint)
        {
            if(player_Controller.IsOnSavePoint) conditionMet = true;
        }

        if (checkSavePoint == CheckSavePoint.ExitSavePoint)
        {
            if (!player_Controller.IsOnSavePoint) conditionMet = true;
        }
    }
}