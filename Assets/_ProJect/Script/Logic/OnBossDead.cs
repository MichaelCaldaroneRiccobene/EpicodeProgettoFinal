using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnBossDead : MonoBehaviour
{
    [SerializeField] private CanvasGroup victoryCanvasGroup;
    [SerializeField] private TextMeshProUGUI textWin;
    [SerializeField] private AudioList_SO clipWin;
    [SerializeField] private string menuSceneName = "Menu";
    [SerializeField] private float timeToShowVictoryCanvas = 5f;

    private SaveData saveData = new SaveData();

    public void Victory()
    {
        Player_UI.Instance.ShowUIOrHide(false);
        if(clipWin) clipWin.PlaySound(transform);


        saveData.NewGame();
        SaveSystem.Save(saveData);

        textWin.alpha = 0f;
        textWin.DOFade(1f, timeToShowVictoryCanvas * 2f);

        victoryCanvasGroup.DOFade(1f, timeToShowVictoryCanvas).OnComplete(() =>
        {
            Utility.DelayAction(this, 1, () =>
            {
                SceneManager.LoadScene(menuSceneName);
            });
        });
    }
}
