using Utils.Singleton;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackScreenController : Singleton<BlackScreenController>
{
    [SerializeField] private GameObject _blackScreen_Panel;
    [SerializeField] private CanvasGroup _blackScreen_CanvasGroup;
    private float _blackCameraFadeTime = 0.5f;

    private float _blackFadeTime => Helpers.blackFadeTime;
    private UIManager _uiManager => UIManager.I;
    private ThirdPersonController _thirdPersonController => ThirdPersonController.I;
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

    #region Fade in and out

    public void FadeInBlack()
    {
        _blackScreen_Panel.SetActive(true);
        _blackScreen_CanvasGroup.DOFade(1, _blackFadeTime);
    }

    public void FadeOutBlack()
    {
        _blackScreen_CanvasGroup.DOFade(0, _blackFadeTime);
        _blackScreen_Panel.SetActive(false);
    }

    #endregion

    #region Fades with scenes
    public void FadeInSceneStart()
    {
        _blackScreen_Panel.SetActive(true);
        _blackScreen_CanvasGroup.alpha = 1f;
        _blackScreen_CanvasGroup.DOFade(0, _blackFadeTime).onComplete = () => _blackScreen_Panel.SetActive(false);
    }

    public void FadeOutScene(string nextScene)
    {
        _blackScreen_CanvasGroup.alpha = 0f;
        _blackScreen_Panel.SetActive(true);
        _blackScreen_CanvasGroup.DOFade(1, _blackFadeTime).OnComplete(() => {
            SceneManager.LoadScene(nextScene);
        }).SetUpdate(true);
    }

    #endregion

    #region Fades with panels
    public void FadePanel(GameObject panel, bool estado)
    {
        _blackScreen_Panel.SetActive(true);
        _blackScreen_CanvasGroup.DOFade(1, _blackFadeTime).SetUpdate(true).onComplete = () => {
            panel.SetActive(estado);
            FadeInSceneStart();
        };
    }
    #endregion

    #region Fades with cameras

    public void CameraChangeFade(GameObject cameraOff, GameObject cameraOn)
    {
        _uiManager.DisableInput();
        _blackScreen_Panel.SetActive(true);
        _thirdPersonController.DisableInputs();
        if(_uiManager.GetActiveCamera() != cameraOn)
        {
            _blackScreen_CanvasGroup.DOFade(1, _blackCameraFadeTime).OnComplete(() =>
            {
                cameraOff.SetActive(false);
                cameraOn.SetActive(true);
                _blackScreen_CanvasGroup.DOFade(0, _blackCameraFadeTime).OnComplete(() =>
                {
                    _thirdPersonController.EnableInputs();
                    _blackScreen_Panel.SetActive(false);
                    _uiManager.EnableInput();
                });
            });
        }
    }

    #endregion

    #region GET

    public bool GetBlackScreenOn()
    {
        return _blackScreen_CanvasGroup.alpha == 1;
    }

    public bool GetBlackScreenOff()
    {
        return _blackScreen_CanvasGroup.alpha == 0;
    }

    #endregion
}