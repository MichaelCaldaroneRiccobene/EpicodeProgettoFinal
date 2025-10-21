using System.Collections;
using UnityEngine;

public class Human_Basic_Controller : MonoBehaviour, I_Team, I_Attack, I_Token
{
    [Header("Setting I_Team")]
    [SerializeField] private int idTeam = 1;

    [Header("Setting I_Token")]
    [SerializeField] private float restorTokenTime = 8;
    [SerializeField] private int tokenStart = 2;

    private Transform target;
    private LifeController lifeController;
    private Stamina_Controller staminaController;

    private int token;
    private bool isAttack;

    private WaitForSeconds waitTimeRestorToken;

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

        if(lifeController) lifeController.UpdateLifeHud += UpdateLifeHud;
        staminaController.UpdateStaminaHud += UpdateStaminaHud;

        token = tokenStart;
        waitTimeRestorToken = new WaitForSeconds(restorTokenTime);
    }

    public virtual void UpdateLifeHud(int life,int maxLife) { }
    public virtual void UpdateStaminaHud(int stamina, int maxStamina) { }

    public virtual void OnDisable()
    {
        if(lifeController != null) lifeController.UpdateLifeHud -= UpdateLifeHud;
        if(staminaController != null) staminaController.UpdateStaminaHud -= UpdateStaminaHud;
    }

    #region Interface
    #region I_Team
    public int GetTeam() => idTeam;
    #endregion

    #region I_Attack
    public void SetIsAttack(bool value) => isAttack = value;

    public bool GetIsAttack() => isAttack;
    #endregion

    #region I_Token
    public int GetToken() => token;

    public void RemoveToken()
    {
        token--;
        if(token < 0) token = 0;
        StartCoroutine(RestorTokenRotuine());
    }

    private IEnumerator RestorTokenRotuine()
    {
        yield return waitTimeRestorToken;
        token++;
        if(token > tokenStart) token = tokenStart;
    }

    #endregion
    #endregion
}
