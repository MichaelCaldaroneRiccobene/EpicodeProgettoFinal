using System;
using UnityEngine;

public class Human_BasicInput : MonoBehaviour
{
    public Action<float, float> OnHorizontalAndVerticalInput;

    public Action OnWalk;

    public Action OnAttack;
    public Action <bool> OnDefence;

    public Action OnRoll;
}
