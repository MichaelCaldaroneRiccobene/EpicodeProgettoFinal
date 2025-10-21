using System;
using UnityEngine;

public class Player_Input : Human_BasicInput
{
    private float horizontalInput;
    private float verticalInput;

    private I_Damageble lifeController;

    public Action OnSerchEnemy;
    public Action OnChangeEnemy;

    private void Start()
    {
        lifeController = GetComponent<I_Damageble>();
    }

    private void Update() => InputPlayer();

    private void InputPlayer()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        OnHorizontalAndVerticalInput?.Invoke(horizontalInput, verticalInput);

        //             MovementCharacter            

        //Walk
        if (Input.GetKeyDown(KeyCode.X)) OnWalk?.Invoke();

        //AttacksList
        if (Input.GetMouseButton(0)) OnAttack?.Invoke();// Attacco Leggero

        //Defence
        if (Input.GetMouseButton(1)) OnDefence?.Invoke(true);
        if (Input.GetMouseButtonUp(1)) OnDefence?.Invoke(false);

        //Roll
        if (Input.GetKeyDown(KeyCode.Space)) OnRoll?.Invoke();

        //Aim
        if (Input.GetKeyDown(KeyCode.Tab)) OnSerchEnemy?.Invoke();
        if (Input.GetKeyDown(KeyCode.E)) OnChangeEnemy?.Invoke();

        if (Input.GetKeyDown(KeyCode.Alpha1)) Time.timeScale = 0.05f;
        if (Input.GetKeyDown(KeyCode.Alpha2)) Time.timeScale = 1f;
    }
}
