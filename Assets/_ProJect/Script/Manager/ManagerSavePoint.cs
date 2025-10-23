using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSavePoint : GenericSingleton<ManagerSavePoint>
{
    private Player_Controller player_Controller;

    private SavePoint currentSavePoint;
    private List<SavePoint> savePoints = new List<SavePoint>();

    private List<SavePoint> findSaveInWorld = new List<SavePoint>();

    private SaveData saveData = new SaveData();

    public override void Awake()
    {
        base.Awake();
        findSaveInWorld.AddRange(FindObjectsOfType<SavePoint>());

    }


    private void Start()
    {
        Load();
        if (currentSavePoint)
        {
            player_Controller.transform.position = currentSavePoint.SpawnPoint.position;
            player_Controller.transform.rotation = currentSavePoint.SpawnPoint.rotation;
        }

    }

    private void Load()
    {
        saveData = SaveSystem.Load();

        if (saveData == null)
        {
            saveData = new SaveData();
            return;
        }

        savePoints.Clear();

        foreach (string savedName in saveData.IDCurrentSavePoints)
        {
            foreach (SavePoint save in findSaveInWorld)
            {
                if (save.Name == savedName)
                {
                    savePoints.Add(save);
                    break;
                }
            }
        }

        foreach (SavePoint save in findSaveInWorld)
        {
            if (save.Name == saveData.IDCurrentSavePoint)
            {
                currentSavePoint = save;
                break;
            }
        }
    }

    private void Save()
    {
        if (currentSavePoint != null) saveData.IDCurrentSavePoint = currentSavePoint.Name;


        saveData.IDCurrentSavePoints.Clear();
        foreach (SavePoint save in savePoints)
        {
            if (!saveData.IDCurrentSavePoints.Contains(save.Name)) saveData.IDCurrentSavePoints.Add(save.Name);
        }
        SaveSystem.Save(saveData);
    }

    public void SetPlayer(Player_Controller player_Controller) => this.player_Controller = player_Controller;

    public void SetCurrentSavePlayer(SavePoint savePoint)
    {
        currentSavePoint = savePoint;

        if(!savePoints.Contains(savePoint)) savePoints.Add(savePoint);

        Save();
    }

    public List<SavePoint> GetSavePoits => savePoints;

    public SavePoint GetCurrentSavePoint => currentSavePoint;
}