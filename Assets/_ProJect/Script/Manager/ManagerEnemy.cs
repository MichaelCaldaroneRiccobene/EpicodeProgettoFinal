using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerEnemy : GenericSingleton<ManagerEnemy>
{
    private List<EnemyRespawn> enemyRespawnList = new List<EnemyRespawn>();

    public void RegisterEnemyRespawn(EnemyRespawn enemyRespawn)
    {
        if (!enemyRespawnList.Contains(enemyRespawn)) enemyRespawnList.Add(enemyRespawn);
    }

    public void RespawnAllEnemies()
    {
        foreach (EnemyRespawn enemyRespawn in enemyRespawnList)
        {
            if (enemyRespawn.EnableOnSpot)
            {
                enemyRespawn.gameObject.SetActive(false);
                enemyRespawn.gameObject.SetActive(true);
            }
        }
    }
}
