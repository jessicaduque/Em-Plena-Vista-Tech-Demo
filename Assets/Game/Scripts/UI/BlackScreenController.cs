using Utils.Singleton;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackScreenController : Singleton<BlackScreenController>
{
    [SerializeField] private GameObject _blackScreen_Panel;
    [SerializeField] private CanvasGroup _blackScreen_CanvasGroup;
    private float _blackCameraFadeTime = 0.2f;
    private float _blackFadeTime => Helpers.blackFadeTime;

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
        _blackScreen_Panel.SetActive(true);
        _blackScreen_CanvasGroup.alpha = 1f;
        _blackScreen_CanvasGroup.DOFade(0, _blackFadeTime).onComplete = () => _blackScreen_Panel.SetActive(false);
    }

    public void FadeOutScene(string nomeScene)
    {
        _blackScreen_Panel.SetActive(true);
        _blackScreen_CanvasGroup.DOFade(1, _blackFadeTime).OnComplete(() => SceneManager.LoadScene(nomeScene)).SetUpdate(true);
    }

    public void RestartGame()
    {
        _blackScreen_Panel.SetActive(true);
        _blackScreen_CanvasGroup.DOFade(1, _blackFadeTime).OnComplete(() => SceneManager.LoadScene("Main")).SetUpdate(true);
    }

    #endregion

    #region Fades with panels
    public void FadePanel(GameObject panel, bool estado)
    {
        _blackScreen_Panel.SetActive(true);
        _blackScreen_CanvasGroup.DOFade(1, _blackFadeTime).onComplete = () => {
            panel.SetActive(estado);
            FadeInSceneStart();
        };
    }
    #endregion

    #region Fades with cameras
    
    public void CameraChangeFade(GameObject cameraOff, GameObject cameraOn)
    {
        _blackScreen_Panel.SetActive(true);
        ThirdPersonController.I.DisableInputs();
        _blackScreen_CanvasGroup.DOFade(1, _blackCameraFadeTime).OnComplete(() =>
        {
            cameraOff.SetActive(false);
            cameraOn.SetActive(true);
            _blackScreen_CanvasGroup.DOFade(0, _blackCameraFadeTime).OnComplete(() => 
            {
                ThirdPersonController.I.EnableInputs();
                _blackScreen_Panel.SetActive(false); 
            });
        });
        
    }

    #endregion
}