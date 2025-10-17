using UnityEngine;

public class Human_Basic_Controller : MonoBehaviour, I_Team, I_Attack
{
    [Header("Setting I_Team")]
    [SerializeField] private int idTeam = 1;

    private Transform target;
    private LifeController lifeController;
    private Stamina_Controller staminaController;

    private bool isAttack;

    public bool HasTarget => target != null;


    public bool IsonRollMode { get; set; }
    public bool IsOnBlockMode { get; set; }
    public bool IsOnNotHitReact {  get; set; }

    public virtual void SetTarget(Transform target) => this.target = target;
    public virtual Transform GetTarget() => target;

    

    public virtual void Awake()
    {
        lifeController = GetComponent<LifeController>();
        staminaController = GetComponent<Stamina_Controller>();

        lifeController.UpdateLifeHud += UpdateLifeHud;
        staminaController.UpdateStaminaHud += UpdateStaminaHud;
    }

    public virtual void UpdateLifeHud(int life,int maxLife) { }
    public virtual void UpdateStaminaHud(int stamina, int maxStamina) { }

    public virtual void OnDisable()
    {
        if(lifeController != null) lifeController.UpdateLifeHud -= UpdateLifeHud;
        if(staminaController != null) staminaController.UpdateStaminaHud -= UpdateStaminaHud;
    }

    public int GetTeam() => idTeam;

    public void SetIsAttack(bool value) => isAttack = value;

    public bool GetIsAttack() => isAttack;
}
