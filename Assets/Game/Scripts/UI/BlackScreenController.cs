using Utils.Singleton;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackScreenController : Singleton<BlackScreenController>
{
    [SerializeField] public GameObject blackScreen_Panel;
    [SerializeField] public CanvasGroup blackScreen_CanvasGroup;
    private float tempoFadePreto => Helpers.tempoPretoFade;

    protected override void Awake()
    {
        base.Awake();

        Time.timeScale = 1;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FadeInSceneStart();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region Fades with scenes
    public void FadeInSceneStart()
    {
        blackScreen_Panel.SetActive(true);
        blackScreen_CanvasGroup.alpha = 1f;
        blackScreen_CanvasGroup.DOFade(0, tempoFadePreto).onComplete = () => TelaPretaPanel.SetActive(false);
    }

    public void FadeOutScene(string nomeScene)
    {
        blackScreen_Panel.SetActive(true);
        blackScreen_CanvasGroup.DOFade(1, tempoFadePreto).OnComplete(() => SceneManager.LoadScene(nomeScene)).SetUpdate(true);
    }

    #endregion

    #region Fades with panels
    public void FadePanel(GameObject panel, bool estado)
    {
        blackScreen_Panel.SetActive(true);
        blackScreen_CanvasGroup.DOFade(1, tempoFadePreto).onComplete = () => {
            panel.SetActive(estado);
            FadeInSceneStart();
        };
    }
    #endregion

    #region Fades with scenes
    public void RestartGame()
    {
        blackScreen_Panel.SetActive(true);
        blackScreen_CanvasGroup.DOFade(1, tempoFadePreto).OnComplete(() => SceneManager.LoadScene("Main")).SetUpdate(true);
    }

    #endregion
}