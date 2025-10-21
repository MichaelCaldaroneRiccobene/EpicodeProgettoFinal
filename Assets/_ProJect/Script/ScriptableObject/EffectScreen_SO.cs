using UnityEngine;
[CreateAssetMenu(fileName = "EffectScreen && CameraShake")]
public class EffectScreen_SO : ScriptableObject
{
    public float ShockEffectStart = 1f;
    public float ShockEffectEnd = 0.15f;
    public float ShockEffectFrame = 25f;

    public float ShakeCameraTime = 0.2f;
    public float ShakeCameraForce= 3;
    public float ShakeCameraMaxDistance = 10;

    public void ShockEffect(MonoBehaviour owner) => Utility.ShockEffect(owner, ShockEffectStart, ShockEffectEnd, ShockEffectFrame);

    public void ShakeCamera(Vector3 position) => CameraShake.Instance.OnCameraShake(position, ShakeCameraTime, ShakeCameraForce, ShakeCameraMaxDistance);
}
