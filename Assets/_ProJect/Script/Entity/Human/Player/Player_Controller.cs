using UnityEngine;

public class Player_Controller : Human_Basic_Controller
{
    [SerializeField] private GameObject meshPlayer;

    public Vector3 StartPosition { get; set; }
    public Quaternion StartRotation { get; set; }

    public GameObject MeshPlayer => meshPlayer;
    public bool IsOnSavePoint {  get; set; }

    public override void Awake()
    {
        base.Awake();
        ManagerSavePoint.Instance.SetPlayer(this);

        StartPosition = transform.position;
        StartRotation = transform.rotation;
    }

    override public void OnEnable()
    {
        base.OnEnable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void UpdateLifeHud(int life, int maxLife) => Player_UI.Instance.UpdateLife(life, maxLife);

    public override void UpdateStaminaHud(int stamina, int maxStamina) => Player_UI.Instance.UpdateStamina(stamina, maxStamina);

}
