using System;
using UnityEngine;
using UnityEngine.Events;

public class BossTriggerEnter : MonoBehaviour
{
    public Action BossTrigger;

    private bool isTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if(isTriggered) return;
        if (other.CompareTag("Player"))
        {
            BossTrigger?.Invoke();
            isTriggered = true;
        }
    }

    public void RestorTrigger() => isTriggered = false;
}
