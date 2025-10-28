using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class State_Destroy : AbstractState
{
    public override void StateEnter()
    {
        Destroy(controller.gameObject);
    }

    public override void StateLeave()
    {

    }
}
