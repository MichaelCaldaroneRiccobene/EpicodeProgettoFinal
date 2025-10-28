using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyRespawn : MonoBehaviour
{
    [SerializeField] private bool enableOnSpot = true;

    private Vector3 startPosition;
    private Quaternion startRotation;

    public UnityEvent OnMeDisable;

    public bool EnableOnSpot => enableOnSpot;

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void Start() => AddMeInTheList();

    private void OnEnable()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    private void OnDisable() => OnMeDisable?.Invoke();

    private void AddMeInTheList() => ManagerEnemy.Instance.RegisterEnemyRespawn(this);
}
