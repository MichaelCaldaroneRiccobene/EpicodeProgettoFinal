using UnityEngine;

public class Human_Basic_Controller : MonoBehaviour, I_Team
{
    [Header("Setting I_Team")]
    [SerializeField] private int idTeam = 1;

    private Transform target;
    public bool HasTarget => target != null;

    public bool IsOnBlockMode { get; set; }
    public bool IsOnNotHitReact {  get; set; }

    public virtual void SetTarget(Transform target) => this.target = target;
    public virtual Transform GetTarget() => target;

    public int GetTeam() => idTeam;
}
