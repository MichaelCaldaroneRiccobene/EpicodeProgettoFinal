using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraShake : GenericSingleton<CameraShake>
{
    private CinemachineFreeLook freeLookCamera;
    private CinemachineBasicMultiChannelPerlin[] noiseComponents;

    void Start()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        noiseComponents = new CinemachineBasicMultiChannelPerlin[3];

        for (int i = 0; i < 3; i++)
        {
            noiseComponents[i] = freeLookCamera.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        SetAmplitudeGain(0);
    }

    private void SetAmplitudeGain(float value)
    {
        foreach (CinemachineBasicMultiChannelPerlin noise in noiseComponents) noise.m_AmplitudeGain = value;
    }

    public void OnCameraShake(Vector3 position, float timer, float maxIntensity, float maxDistance)
    {
        StartCoroutine(OnCameraShakeRoutine(position, timer, maxIntensity, maxDistance));
    }

    private IEnumerator OnCameraShakeRoutine(Vector3 position, float duration, float maxIntensity, float maxDistance)
    {
        if (freeLookCamera.m_Follow == null) yield break;

        float distanceToCamera = Vector3.Distance(position, freeLookCamera.m_Follow.position);
        float distanceFactor = 1 - Mathf.Clamp01(distanceToCamera - maxDistance);

        SetAmplitudeGain(maxIntensity * distanceFactor);
        yield return new WaitForSeconds(duration);

        SetAmplitudeGain(0);
    }

    private void OnDestroy() => StopAllCoroutines();
}
