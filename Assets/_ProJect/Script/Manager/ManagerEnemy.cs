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

                if (enemyRespawn.TryGetComponent(out LifeController life)) life.RestorHp();
                if (enemyRespawn.TryGetComponent(out Stamina_Controller stamina)) stamina.RestorStamina();
            }
        }
    }
}
