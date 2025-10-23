using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioList")]
public class AudioList_SO : ScriptableObject
{
    public AudioParameters AudioParameters;

    public void PlaySound(Transform parent)
    {
        if (ManagerPooling.Instance) ManagerPooling.Instance.GetAudioFromPool(
            ObjTypePoolling.Audio, parent.position, Quaternion.identity,
            AudioParameters.AudioClips[Random.Range(0, AudioParameters.AudioClips.Length)],
            AudioParameters.Volume, AudioParameters.MinPitch, AudioParameters.MaxPitch,
            AudioParameters.IsRandomPitch, AudioParameters.Is3D);
    }
}

[System.Serializable]
public class AudioParameters
{
    public AudioClip[] AudioClips;
    public float Volume = 1f;
    public float MinPitch = 1f;
    public float MaxPitch = 1f;

    public bool IsRandomPitch = true;
    public bool Is3D = true;
}
