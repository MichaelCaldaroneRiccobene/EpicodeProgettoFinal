using System.Collections.Generic;
using UnityEngine;

public class Player_SerchTarget : MonoBehaviour
{
    [Header("Setting Player_SerchTarget")]
    [SerializeField] private float distanceForSerchEnemy = 10f;
    [SerializeField] private Transform head;
    [SerializeField] private LayerMask wallLayer;

    private Player_Controller player_Controller;
    private Player_Input player_Input;

    private LifeController lifeControllerTarget;
    private Transform possibleTarget;
    private I_Target currentITarget;

    private List<Transform> enemyTransforms = new List<Transform>();

    private void Awake()
    {
        player_Controller = GetComponent<Player_Controller>();
        player_Input = GetComponent<Player_Input>();
    }

    private void OnEnable()
    {
        SetAction();
    }

    private void Update()
    {
        if (!player_Controller.HasTarget) return;

        if(lifeControllerTarget && lifeControllerTarget.IsDead())
        {
            ClearTarget();
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, player_Controller.GetTarget().position);
        if (OnSeeTarget(player_Controller.GetTarget())) ClearTarget();

        if (player_Controller.HasTarget) Debug.DrawLine(head.position, player_Controller.GetTarget().position + new Vector3(0,head.position.y,0), Color.yellow);
        if (distanceToTarget > distanceForSerchEnemy) ClearTarget();
    }

    private void SetAction()
    {
        if (player_Input)
        {
            player_Input.OnSerchEnemy += OnSerchEnemy;
            player_Input.OnChangeEnemy += ChangeTarget;
        }
    }

    private void OnSerchEnemy()
    {
        if (player_Controller.IsonRollMode) return;

        if (player_Controller.HasTarget) ClearTarget();
        else OnSphereSerch(true);
    }

    private void OnSphereSerch(bool canClearTarget)
    {
        float closestDistance = Mathf.Infinity;

        SerchAllTargets();

        foreach (Transform enemy in enemyTransforms)
        {
            if (player_Controller.GetTarget()) if (enemy == player_Controller.GetTarget()) continue;

            Vector3 hitDirection = (enemy.position - transform.position).normalized;
            float dot = Vector3.Dot(Camera.main.transform.forward, hitDirection);

            if (dot < 0) continue;

            float distance = Vector3.Distance(transform.position, enemy.position);
            if (distance < closestDistance)
            {
                if (OnSeeTarget(enemy)) continue;

                closestDistance = distance;
                possibleTarget = enemy;
            }
        }
        if (canClearTarget) if (enemyTransforms.Count == 0) ClearTarget();

        if(possibleTarget != null) SetTargetPlayer(possibleTarget);
    }

    private void ChangeTarget()
    {
        if (!player_Controller.HasTarget) return;

        if(currentITarget != null) currentITarget.SetOnOffTargetHud(false);
        currentITarget = null;

        Transform currentTarget = player_Controller.GetTarget();
        OnSphereSerch(false);

        if (enemyTransforms.Count == 0) SetTargetPlayer(currentTarget);
    }

    private void SetTargetPlayer(Transform currentTarget)
    {
        if (currentTarget.TryGetComponent(out I_Target target))
        {
            currentITarget = target;
            currentITarget.SetOnOffTargetHud(true);
            if(currentTarget.TryGetComponent(out LifeController life))
            {
                lifeControllerTarget = life;
                if (lifeControllerTarget.IsDead())
                {
                    ClearTarget();
                    return;
                }
            }
        }

        player_Controller.SetTarget(currentTarget);
    }

    private void ClearTarget()
    {
        player_Controller.SetTarget(null);
        if (currentITarget != null)
        {
            currentITarget.SetOnOffTargetHud(false);
            currentITarget = null;
            lifeControllerTarget = null;
        }
    }


    private void SerchAllTargets()
    {
        enemyTransforms.Clear();

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, distanceForSerchEnemy, transform.forward, 0f);

        foreach (RaycastHit hit in hits) if (hit.transform.TryGetComponent(out I_Team i_Team))
            {
                if (i_Team.GetTeam() == player_Controller.GetTeam()) continue;
                enemyTransforms.Add(hit.transform);
            }
    }

    private bool OnSeeTarget(Transform transform) => Physics.Linecast(head.position, transform.position, wallLayer);

    private void OnDisable()
    {
        if (player_Input != null)
        {
            player_Input.OnSerchEnemy -= OnSerchEnemy;
            player_Input.OnChangeEnemy -= ChangeTarget;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceForSerchEnemy);
    }
}
