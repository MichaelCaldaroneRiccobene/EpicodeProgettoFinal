using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum TypeUI { Resume,Continue,Option,Credit,ReturnMenu,Quit,None }
[System.Serializable]
public class ButtonList
{
    public Button[] buttons;
    public TypeUI Type;
}

[System.Serializable]
public class MenuPanel
{
    public TypeUI panelType;
    public GameObject panelObj;
}

public class ManagerMenu : GenericSingleton<ManagerMenu>
{
    [SerializeField] protected GameObject pannelAll;
    [SerializeField] protected CanvasGroup canvasBlackScreen;
    [SerializeField] protected float timeScreenBlackStart = 10f;

    [SerializeField] protected string nameLevel = "Level1";

    public bool EnablePannelAll;

    protected SaveData saveData = new SaveData();
    protected bool hasSave;

    public override void Awake()
    {
        base.Awake();
        Utility.FadeBlackOrOutBlack(canvasBlackScreen, 0, timeScreenBlackStart, Ease.InQuad,gameObject, () => { });
        Load();

        SetupButtons();

        pannelAll.SetActive(EnablePannelAll);
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

        if (saveData.IDCurrentSavePoint != string.Empty) hasSave = true;
    }

    public virtual void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        OnAnimationBackGround();
        SetUpPannel(TypeUI.ReturnMenu);
    }

    public virtual void EnableDisableMenu() { }
    #region SetUpPannels

    [Header("Setting Panels")]
    [SerializeField] protected MenuPanel[] menuPanels;
    [SerializeField] protected float timeSpawnPanel = 1f;
    [SerializeField] protected float timeSpawnButton = 1f;

    public virtual void SetUpPannel(TypeUI panelMenu)
    {
        foreach (MenuPanel panel in menuPanels)
        {
            panel.panelObj.SetActive(false);

            if (panel.panelType == panelMenu)
            {
                panel.panelObj.SetActive(panel.panelType == panelMenu);           
                panel.panelObj.transform.localScale = Vector3.zero;

                Button[] buttonsInPannel = panel.panelObj.GetComponentsInChildren<Button>();
                foreach (Button btn in buttonsInPannel)
                {
                    btn.transform.localScale = Vector3.zero;
                    btn.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    btn.gameObject.SetActive(false);
                }

                Slider[] slidersInPannel = panel.panelObj.GetComponentsInChildren<Slider>();
                if(slidersInPannel != null && slidersInPannel.Length > 0)
                {
                    foreach (Slider slider in slidersInPannel)
                    {
                        slider.transform.localScale = Vector3.zero;
                        slider.transform.localRotation = Quaternion.Euler(Vector3.zero);
                        slider.gameObject.SetActive(false);
                    }
                }

                panel.panelObj.transform.DOScale(Vector3.one, timeSpawnPanel).SetEase(Ease.OutBack).SetLink(gameObject).OnComplete(() =>
                {
                    foreach (Button btn in buttonsInPannel) btn.gameObject.SetActive(true);
                    if(slidersInPannel != null && slidersInPannel.Length > 0) { foreach (Slider sls in slidersInPannel) sls.gameObject.SetActive(true); }

                    StartCoroutine(AnimationSpawnButtonsInPannelRoutine(buttonsInPannel));
                    StartCoroutine(AnimationSpawnSlidersInPannelRoutine(slidersInPannel));
                });
            }
            
        }
    }

    private IEnumerator AnimationSpawnButtonsInPannelRoutine(Button[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Button btn = buttons[i];
            btn.transform.DOScale(Vector3.one, timeSpawnButton).SetEase(Ease.OutBounce).SetLink(gameObject);

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator AnimationSpawnSlidersInPannelRoutine(Slider[] sliders)
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            if (sliders != null && sliders.Length > 0)
            {
                Slider sls = sliders[i];
                sls.transform.DOScale(Vector3.one, timeSpawnButton).SetEase(Ease.OutBounce).SetLink(gameObject);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    #endregion
    #region SetUpButtons
    [Header("Setting Buttons")]
    [SerializeField] protected List<ButtonList> buttonList;
    private void SetupButtons()
    {
        if (buttonList == null || buttonList.Count == 0) return;

        foreach (ButtonList list in buttonList)
        {
            if (list.buttons == null || list.buttons.Length == 0) continue;

            foreach (Button button in list.buttons)
            {
                switch (list.Type)
                {
                    case TypeUI.Resume:
                        button.onClick.AddListener(Resume);
                        break;

                    case TypeUI.Continue:
                        button.onClick.AddListener(ContinueGame);
                        button.interactable = hasSave;
                        break;

                    case TypeUI.Option:
                        button.onClick.AddListener(OpenOption);
                        break;

                    case TypeUI.Credit:
                        button.onClick.AddListener(OpenCredits);
                        break;

                    case TypeUI.ReturnMenu:
                        button.onClick.AddListener(ReturMenu);
                        break;

                    case TypeUI.Quit:
                        button.onClick.AddListener(QuitGame);
                        break;
                }
            }
        }
    }

    public virtual void Resume()
    {
        if(saveData != null)
        {
            saveData.NewGame();
            SaveSystem.Save(saveData);
        }
        ContinueGame();
    }

    public virtual void ContinueGame()
    {
        Utility.FadeBlackOrOutBlack(canvasBlackScreen, 1, 1, Ease.InQuad, gameObject, () => { SceneManager.LoadScene(nameLevel); });        
    }

    public virtual void OpenOption() => SetUpPannel(TypeUI.Option);   
    public virtual void OpenCredits() => SetUpPannel(TypeUI.Credit); 
    public virtual void ReturMenu() => SetUpPannel(TypeUI.ReturnMenu);  
    public virtual void QuitGame() => Application.Quit();

    #endregion
    #region Animatios

    [Header("Animations Menu")]
    [SerializeField] private Color firstColorMenu;
    [SerializeField] private Color secondColorMenu;

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private float originalSizeTitle = 150f;
    [SerializeField] private float increaseSizeTitle = 160f;

    [SerializeField] private AudioList_SO clipBackgroundMenu;
    [SerializeField] private float timeChangeColorMenu = 10f;

    private void OnAnimationBackGround()
    {
        AnimationColorBackGround();
        AnimationText();
    }

    private void AnimationColorBackGround()
    {
        Camera.main.backgroundColor = firstColorMenu;
        int interval = 0;

        DOTween.To(() => Camera.main.backgroundColor, x => Camera.main.backgroundColor = x, secondColorMenu, timeChangeColorMenu)
        .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetLink(gameObject).OnStepComplete(() =>
        {
            if (interval == 0)
            {
                clipBackgroundMenu.PlaySound(transform);
                interval++;
            }
            else interval = 0;
        });
    }

    private void AnimationText()
    {
        title.fontSize = originalSizeTitle;

        DOTween.To(() => title.fontSize, x => title.fontSize = x, increaseSizeTitle, timeChangeColorMenu)
            .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetLink(gameObject);
    }
    #endregion

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
}
