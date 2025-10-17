using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    [Header("Setting PlayerCamera")]
    [SerializeField] private Player_Controller player_Controller;
    [SerializeField] private float blendSpeed = 5f;

    [SerializeField] private float blendCamOffTargetZero = 1f;
    [SerializeField] private float blendCamOffTargetOne = 0f;

    [SerializeField] private float blendCamOnTargetZero = 1f;
    [SerializeField] private float blendCamOnTargetOne = 0.35f;

    [SerializeField] private CinemachineMixingCamera mixingCamera;
    [SerializeField] private CinemachineFreeLook freeLook;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Update()
    {
        float targetWeightZero = player_Controller.HasTarget ? blendCamOnTargetZero : blendCamOffTargetZero;
        float targetWeightOne = player_Controller.HasTarget ? blendCamOnTargetOne : blendCamOffTargetOne;

        float currentWeightZero = mixingCamera.GetWeight(0);
        float newWeightZero = Mathf.Lerp(currentWeightZero, targetWeightZero, Time.deltaTime * blendSpeed);
        mixingCamera.SetWeight(0, newWeightZero);

        float currentWeightOne = mixingCamera.GetWeight(1);
        float newWeightOne = Mathf.Lerp(currentWeightOne, targetWeightOne, Time.deltaTime * blendSpeed);
        mixingCamera.SetWeight(1, newWeightOne);

        virtualCamera.m_LookAt = player_Controller.GetTarget();

        freeLook.m_RecenterToTargetHeading.m_enabled = player_Controller.HasTarget ? true : false;
        freeLook.m_YAxisRecentering.m_enabled = player_Controller.HasTarget ? true : false;
    }
}
