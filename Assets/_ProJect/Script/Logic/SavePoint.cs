using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private string namePlace = "NAMEEEEEE";
    [SerializeField] private Sprite imagePlace;


    [SerializeField] private CanvasGroup canvasGroupBlack;
    [SerializeField] private CanvasGroup cavansUI;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private Button buttonForSavePoints;
    [SerializeField] private Transform scrollParent;

    [SerializeField] private GameObject pannelUI;
    [SerializeField] private GameObject meshTest;
    [SerializeField] private Button sitUpButton;
    [SerializeField] private float timeForDoActions = 0.5f;

    private Player_Controller currentPlayerController;
    private List<Button> buttonsList = new List<Button>();
    private bool isOnSavePoint;

    public Transform SpawnPoint => spawnPoint;
    public string Name => namePlace;
    public Sprite Image => imagePlace;

    private void Start()
    {
        meshTest.SetActive(false);
        pannelUI.SetActive(false);

        sitUpButton.onClick.AddListener(SitUp);
    }


    private void SitDown(Player_Controller player_Controller)
    {
        currentPlayerController = player_Controller;
        ManagerSavePoint.Instance.SetCurrentSavePlayer(this);
        isOnSavePoint = true;


        canvasGroupBlack.DOFade(1, timeForDoActions).OnComplete(() =>
        {
            SpawnButtonsForSavePoint();

            pannelUI.SetActive(true);
            meshTest.SetActive(true);

            currentPlayerController.MeshPlayer.SetActive(false);
            currentPlayerController.IsOnSavePoint = true;

            player_Controller.transform.position = spawnPoint.position;
            player_Controller.transform.rotation = spawnPoint.rotation;

            cavansUI.DOFade(1, timeForDoActions).OnComplete(() =>
            {
                canvasGroupBlack.DOFade(0, timeForDoActions);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            });
        }); 
    }

    public void SitUp()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cavansUI.DOFade(0, timeForDoActions).OnComplete(()=>
        {
            canvasGroupBlack.DOFade(1, timeForDoActions).OnComplete(() =>
            {
                SitUpExitActions();

                currentPlayerController.IsOnSavePoint = false;
                currentPlayerController.MeshPlayer.SetActive(true);

                canvasGroupBlack.DOFade(0, timeForDoActions);
            });
        });
    }

    public void SitUpExitActions()
    {
        ClearButtos();

        meshTest.SetActive(false);
        pannelUI.SetActive(false);
        isOnSavePoint = false;
    }

    private void SpawnButtonsForSavePoint()
    {
        foreach(SavePoint savePoint in ManagerSavePoint.Instance.GetSavePoits)
        {
            Button button = Instantiate(buttonForSavePoints, scrollParent);
            button.GetComponent<Image>().sprite = savePoint.imagePlace;

            button.onClick.AddListener(() =>
            {
                savePoint.GoNextSavePoint(currentPlayerController,this);
            });

            buttonsList.Add(button);
        }
    }

    private void ClearButtos()
    {
        foreach(Button button in buttonsList) Destroy(button.gameObject);

        buttonsList.Clear();
    }

    public void GoNextSavePoint(Player_Controller playerController,SavePoint savePoint)
    {
        ClearButtos();
        savePoint.SitUpExitActions();

        currentPlayerController = playerController;
        pannelUI.SetActive(false);
        ManagerSavePoint.Instance.SetCurrentSavePlayer(this);

        canvasGroupBlack.DOFade(1, timeForDoActions).OnComplete(() =>
        {
            SpawnButtonsForSavePoint();
            pannelUI.SetActive(true);
            meshTest.SetActive(true);

            currentPlayerController.MeshPlayer.SetActive(false);
            currentPlayerController.IsOnSavePoint = true;

            playerController.transform.position = spawnPoint.position;
            playerController.transform.rotation = spawnPoint.rotation;

            cavansUI.DOFade(1, timeForDoActions).OnComplete(() =>
            {
                canvasGroupBlack.DOFade(0, timeForDoActions);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            });
        });
    }

    private void OnTriggerStay(Collider other)
    {
        if(isOnSavePoint) return;

        if (other.TryGetComponent(out Player_Controller player_Controller))
        {
            if(Input.GetKeyDown(KeyCode.F)) SitDown(player_Controller);
        }
    }
}
