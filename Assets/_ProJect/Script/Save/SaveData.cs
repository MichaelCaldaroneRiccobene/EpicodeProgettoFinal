using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public string IDCurrentSavePoint;
    public List<string> IDCurrentSavePoints = new List<string>();

    public float VolumeMaster = 1f;
    public float VolumeSFX = 1f;
    public float VolumeMusic = 1f;

    public void NewGame()
    {
        IDCurrentSavePoint = string.Empty;
        IDCurrentSavePoints.Clear();
    }
}
