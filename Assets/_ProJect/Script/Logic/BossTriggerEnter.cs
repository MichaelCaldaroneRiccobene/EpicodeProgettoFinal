using System;
using UnityEngine;
using UnityEngine.Events;

public class BossTriggerEnter : MonoBehaviour
{
    public Action BossTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BossTrigger?.Invoke();
        }
    }
}
