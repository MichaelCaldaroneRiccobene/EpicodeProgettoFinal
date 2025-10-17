using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_OnSeeTarget : AbstractTransition
{
    [Header("Setting For See")]
    [SerializeField] private float viewAngleForward = 60;
    [SerializeField] private float viewAngleBack = 120;

    [SerializeField] private float sightDistance = 12;
    [SerializeField] private float sightSenseDistance = 4;

    [SerializeField] private int raySightToAdd = 70;
    [SerializeField] private int raySenseToAdd = 30;

    [SerializeField] private float hight = 1;

    private Npc_Controller npc_Controller;

    public override void SetUp(FSM_Controller controller)
    {
        base.SetUp(controller);
        npc_Controller = controller.GetComponent<Npc_Controller>();
    }

    public override void CheckConditionUpdate()
    {
        TrySee(hight, raySightToAdd, sightDistance, viewAngleForward, transform.forward, Color.yellow);
        TrySee(hight, raySenseToAdd, sightSenseDistance, viewAngleBack, -transform.forward, Color.green);
    }

    private void TrySee(float hight, int rayToAdd, float sightDistance, float viewAngle, Vector3 forward, Color color)
    {
        Vector3 originCast = controller.transform.position + new Vector3(0, hight, 0);
        float deltaAngle = (2 * viewAngle) / (rayToAdd - 1);

        for (int i = 0; i < rayToAdd; i++)
        {
            float curretAngle = -viewAngle + deltaAngle * i;
            Vector3 direction = Quaternion.Euler(0, curretAngle, 0) * forward;

            if (Physics.Raycast(originCast, direction, out RaycastHit hit, sightDistance))
            {
                if (controller.CanSeeDebug) Debug.DrawLine(originCast, hit.point, Color.red, 0.1f);

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent(out I_Team hitEntity))
                    {
                        if (hit.collider.TryGetComponent(out LifeController lifeSistem) && lifeSistem.IsDead()) continue;

                        // Vede Amici :)
                        if (hitEntity.GetTeam() != npc_Controller.GetTeam()) SetTarget(hit.collider.transform); // Vede Nemici :(
                    }
                }
            }
            else if (controller.CanSeeDebug) Debug.DrawRay(originCast, direction * sightDistance, color, 0.1f);
        }
    }

    private void SetTarget(Transform hit)
    {
        npc_Controller.SetTarget(hit);
        conditionMet = true;
    }
}
