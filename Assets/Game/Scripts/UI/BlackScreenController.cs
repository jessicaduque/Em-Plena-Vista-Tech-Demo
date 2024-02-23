using Utils.Singleton;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackScreenController : Singleton<BlackScreenController>
{
    [SerializeField] public GameObject blackScreen_Panel;
    
    CanvasGroup blackScreen_CanvasGroup;
    GameObject Player;
    //TEMPORARIO 
    [SerializeField]GameObject CorpoMonge;

    float blackFadeTime => Helpers.blackFadeTime;
    AudioManager _audioManager => AudioManager.I;

    protected override void Awake()
    {
        base.Awake();

        Player = GameObject.FindGameObjectWithTag("Player");

        blackScreen_CanvasGroup = blackScreen_Panel.GetComponent<CanvasGroup>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FadeInSceneStart();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region Black Fade With Scene

    public void FadeInSceneStart()
    {
        blackScreen_CanvasGroup.alpha = 1f;
        blackScreen_CanvasGroup.DOFade(0, blackFadeTime);
    }

    public void FadeOutScene(string sceneName)
    {
        blackScreen_CanvasGroup.DOFade(1, blackFadeTime).OnComplete(() => SceneManager.LoadScene(sceneName)).SetUpdate(true);
    }

    #endregion

    #region Black Fade With Panel

    public void FadeBlackWithPanel(GameObject panel, bool state)
    {
        blackScreen_CanvasGroup.DOFade(1, blackFadeTime).onComplete = () => {
            panel.SetActive(state);
            FadeInSceneStart();
        };
    }

    #endregion

    #region Public Get

    public CanvasGroup GetBlackPanelCanvasGroup()
    {
        return blackScreen_CanvasGroup;
    }

    #endregion

}