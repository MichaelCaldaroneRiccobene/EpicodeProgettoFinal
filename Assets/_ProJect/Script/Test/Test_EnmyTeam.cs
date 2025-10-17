using UnityEngine;

public class Test_EnmyTeam : MonoBehaviour, I_Team
{
    public int team = 999;
    public int GetTeam() => team;
}
