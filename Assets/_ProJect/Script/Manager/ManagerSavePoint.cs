using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManagerSavePoint : GenericSingleton<ManagerSavePoint>
{
    [SerializeField] private CanvasGroup canvasDead;
    [SerializeField] private TextMeshProUGUI textRespawn;
    [SerializeField] private float increaseSizeTitle = 348f;
    [SerializeField] private float timeCavasDead = 4f;

    [SerializeField] private float timeCanvasDead = 1f;

    [SerializeField] private Player_Controller player_Controller;

    private SavePoint currentSavePoint;
    private List<SavePoint> savePoints = new List<SavePoint>();

    private List<SavePoint> findSaveInWorld = new List<SavePoint>();

    private SaveData saveData = new SaveData();

    public List<SavePoint> GetSavePoits => savePoints;

    public SavePoint GetCurrentSavePoint => currentSavePoint;

    public override void Awake()
    {
        base.Awake();
        findSaveInWorld.AddRange(FindObjectsOfType<SavePoint>());
        canvasDead.alpha = 0f;
    }


    private void Start()
    {
        Load();
        if (currentSavePoint)
        {
            player_Controller.gameObject.SetActive(false);
            Debug.Log("Load Save Point: " + currentSavePoint.Name);

            player_Controller.transform.rotation = currentSavePoint.SpawnPoint.rotation;
            player_Controller.transform.position = currentSavePoint.SpawnPoint.position;

            player_Controller.gameObject.SetActive(true);
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

    public void RestorPlayer()
    {
        textRespawn.fontSize = 0f;
        canvasDead.DOFade(1, timeCanvasDead).OnComplete(() =>
        {
            player_Controller.gameObject.SetActive(false);
            Player_UI.Instance.ShowUIOrHide(false);

            if (currentSavePoint)
            {
                player_Controller.transform.position = currentSavePoint.SpawnPoint.position;
                player_Controller.transform.rotation = currentSavePoint.SpawnPoint.rotation;
            }
            else
            {
                player_Controller.transform.position = player_Controller.StartPosition;
                player_Controller.transform.rotation = player_Controller.StartRotation;
            }

            DOTween.To(() => textRespawn.fontSize, x => textRespawn.fontSize = x, increaseSizeTitle, timeCavasDead)
                .SetEase(Ease.Linear).SetLink(gameObject);

            Utility.DelayAction(this, timeCavasDead, () =>
            {
                player_Controller.gameObject.SetActive(true);
                canvasDead.DOFade(0, timeCanvasDead);
                Player_UI.Instance.ShowUIOrHide(true);

                ManagerEnemy.Instance.RespawnAllEnemies();
            });
        });
    }
}