using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum VelocityNpc { Walk,Jog}

public class Npc_Controller : Human_Basic_Controller, I_Target
{
    [Header("Setting Hud")]
    [SerializeField] private GameObject hud;

    [SerializeField] private Slider sliderLife;
    [SerializeField] private Slider sliderLifeYellow;

    [SerializeField] private Slider sliderStamina;
    [SerializeField] private Slider sliderStaminaYellow;

    [SerializeField] private float delayUpdateYellowLife = 0.5f;
    [SerializeField] private float speedUpdateYellowLife = 0.4f;

    [SerializeField] private GameObject inStoppableAttack;

    [SerializeField] private float walkSpeed = 1.7f;
    [SerializeField] private float jogSpeed = 3.4f;

    private NavMeshAgent agent;

    private bool isUpdateYellowLife;
    private bool isUpdateYellowStamina;

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

    private void Update()
    {
        if (isUpdateYellowLife)
        {
            if (sliderLifeYellow.value > sliderLife.value) sliderLifeYellow.value -= speedUpdateYellowLife * Time.deltaTime;
            else isUpdateYellowLife = false;
        }

        if (isUpdateYellowStamina)
        {
            if (sliderStaminaYellow.value > sliderStamina.value)
            {
                sliderStaminaYellow.value -= speedUpdateYellowLife * Time.deltaTime;
            }
            else if (sliderStaminaYellow.value < sliderStamina.value)
            {
                sliderStaminaYellow.value = sliderStamina.value;
            }
            else if (sliderStaminaYellow.value == sliderStamina.value)
            {
                isUpdateYellowStamina = false;
            }
        }
    }

    public override  void UpdateLifeHud(int life, int maxLife)
    {
        sliderLife.value = (float)life / maxLife;

        if (!isUpdateYellowLife) { Utility.DelayAction(this, delayUpdateYellowLife, () => { isUpdateYellowLife = true; }); }
    }

    public override void UpdateStaminaHud(int stamina, int maxStamina)
    {
        sliderStamina.value = (float)stamina / maxStamina;

        if (!isUpdateYellowStamina) { Utility.DelayAction(this, delayUpdateYellowLife, () => { isUpdateYellowStamina = true; }); }
    }

    public void SetOnOffTargetHud(bool value) => hud.SetActive(value);
    public void EnableDisableInStoppableAttack(bool value) => inStoppableAttack.SetActive(value);
}
