using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum TypeVolume { Master, SFX, Music }

[System.Serializable]
public class SlidersVolume
{
    public Slider slider;
    public TypeVolume typeVolume;
}

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private SlidersVolume[] volumes;

    private SaveData saveData = new SaveData();

    private void Start()
    {
        Load();
    }

    private void Load()
    {
        saveData = SaveSystem.Load();

        if (saveData == null)
        {
            saveData = new SaveData();
            Debug.Log("Save Data Null");
            return;
        }

        foreach (SlidersVolume volume in volumes)
        {
            switch (volume.typeVolume)
            {
                case TypeVolume.Master:
                    volume.slider.value = saveData.VolumeMaster;
                    volume.slider.onValueChanged.AddListener(SetVolumeMaster);

                    SetUpForMixer("VolumeMaster", volume.slider.value);
                    break;
                case TypeVolume.SFX:
                    volume.slider.value = saveData.VolumeSFX;
                    volume.slider.onValueChanged.AddListener(SetVolumeSFX);
                    SetUpForMixer("VolumeSFX", volume.slider.value);
                    break;
                case TypeVolume.Music:
                    //volume.slider.value = saveData.VolumeMusic;
                    //volume.slider.onValueChanged.AddListener(SetVolumeMusic);
                    //SetVolumeMusic(saveData.VolumeMusic);
                    break;
            }
        }
    }

    public void SetVolumeMaster(float volume)
    {
        SetUpForMixer("VolumeMaster", volume);
        saveData.VolumeMaster = volume;
        Save();
    }
    public void SetVolumeSFX(float volume)
    {
        SetUpForMixer("VolumeSFX", volume);
        saveData.VolumeSFX = volume;
        Save();
    }

    //public void SetVolumeMusic(float volume)
    //{
    //    audioMixer.SetFloat("VolumeMusic", Mathf.Log10(volume) * 20);
    //    saveData.VolumeMusic = volume;
    //    Save();
    //}

    private void SetUpForMixer(string name,float volume) => audioMixer.SetFloat(name, Mathf.Log10(volume) * 20);

    private void Save() => SaveSystem.Save(saveData);
}
    