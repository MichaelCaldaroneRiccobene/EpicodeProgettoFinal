using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum VelocityNpc { Walk,Jog}

public class Npc_Controller : Human_Basic_Controller, I_Target
{
    [Header("Setting Hud")]
    [SerializeField] private GameObject hud;
    [SerializeField] private Image imageLife;
    [SerializeField] private Image imageStamina;
    [SerializeField] private GameObject inStoppableAttack;

    [SerializeField] private float walkSpeed = 1.7f;
    [SerializeField] private float jogSpeed = 3.4f;

    private NavMeshAgent agent;

    public float WalkSpeed => walkSpeed;
    public float JogSpeed => jogSpeed;

    public override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        agent.speed = jogSpeed;
        hud.SetActive(false);
        inStoppableAttack.SetActive(false);
    }

    public void SetOnOffTargetHud(bool value) => hud.SetActive(value);

    public override void UpdateLifeHud(int life, int maxLife) => imageLife.fillAmount = (float)life /maxLife;

    public override void UpdateStaminaHud(int stamina, int maxStamina) => imageStamina.fillAmount = (float)stamina/maxStamina;

    public void EnableDisableInStoppableAttack(bool value) => inStoppableAttack.SetActive(value);
}
