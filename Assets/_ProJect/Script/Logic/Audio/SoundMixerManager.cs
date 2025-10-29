using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetVolumeMaster(float volume)
    {
        audioMixer.SetFloat("VolumeMaster",Mathf.Log10(volume) * 20);
    }
    public void SetVolumeSFX(float volume)
    {
        audioMixer.SetFloat("VolumeSFX", Mathf.Log10(volume) * 20);
    }

    public void SetVolumeMusic(float volume)
    {
        audioMixer.SetFloat("VolumeMusic", Mathf.Log10(volume) * 20);
    }
}
    