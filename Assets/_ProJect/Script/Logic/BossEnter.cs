using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        bossMan.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        bossTriggerEnter.BossTrigger -= OnBossTrigger;
    }
}