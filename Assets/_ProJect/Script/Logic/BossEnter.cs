using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnter : MonoBehaviour
{
    [SerializeField] private GameObject bossMan;
    [SerializeField] private BoxCollider fog;

    private BossTriggerEnter bossTriggerEnter;

    private void Awake()
    {
        bossTriggerEnter = GetComponentInChildren<BossTriggerEnter>();
        bossTriggerEnter.BossTrigger += OnBossTrigger;
    }

    private void OnBossTrigger()
    {
        fog.isTrigger = false;
        bossMan.gameObject.SetActive(false);
        if(bossMan.gameObject.TryGetComponent(out NavMeshAgent navMeshAgent)) navMeshAgent.enabled = false;

        Utility.DelayAction(this,0.5f, () =>
        {
            bossMan.gameObject.SetActive(true);
            fog.isTrigger = false;
        });
       
    }

    private void OnDisable()
    {
        bossTriggerEnter.BossTrigger -= OnBossTrigger;
    }
}