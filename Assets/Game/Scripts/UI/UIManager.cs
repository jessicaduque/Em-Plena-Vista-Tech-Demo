using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils.Singleton;
using DG.Tweening;
/// <summary>
/// Manager instance in game main scene to control all UI aspects
/// </summary>
public class UIManager : Singleton<UIManager>
{
    private CanvasGroup _interactionButtonCanvasGroup; // Canvas group for the HUD interaction button indication
    private float _interactionButtonFadeTime = 0.5f; // Time for fade in and out of interaction button indication
    private Tweener _interactionButtonTweener; // Tweener to save animation for interaction button indication
    private GameObject _activeCamera; // Active camera in scene

    [SerializeField] private Image im_interactionButton; // Interaction button Image for indication
    [SerializeField] private Sprite _interactionButtonGamepad;  // Interaction button sprite for the gamepad button
    [SerializeField] private Sprite _interactionButtonKeyboardMouse; // Interaction button sprite for the keyboard/mouse button

    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _endPanel;
    [SerializeField] private Button b_exitGame;

    private BlackScreenController _blackScreenController => BlackScreenController.I;
    private AudioManager _audioManager => AudioManager.I;
    private new void Awake()
    {
        _interactionButtonCanvasGroup = im_interactionButton.GetComponent<CanvasGroup>();
        im_interactionButton.sprite = (Gamepad.all.Count > 0 ? _interactionButtonGamepad : _interactionButtonKeyboardMouse);
    }

    private void Start()
    {
        GameController.I._gamepadConnectedEvent += () => ChangeInteractionButtonSprite(true);
        GameController.I._gamepadDisconnectedEvent += () => ChangeInteractionButtonSprite(false);
    }

    private void ChangeInteractionButtonSprite(bool state)
    {
        im_interactionButton.sprite = (state ? _interactionButtonGamepad : _interactionButtonKeyboardMouse);
    }
    public void ControlInteractionButton(bool state)
    {
        _interactionButtonTweener?.Kill();

        if (state)
        {
            _interactionButtonCanvasGroup.alpha = 0;
            im_interactionButton.enabled = true;
            _interactionButtonTweener = _interactionButtonCanvasGroup.DOFade(1, _interactionButtonFadeTime).SetEase(Ease.InOutSine).OnComplete(() => _interactionButtonCanvasGroup.DOFade(0, _interactionButtonFadeTime)).SetLoops(-1, LoopType.Yoyo);
        }
        else
            _interactionButtonTweener = _interactionButtonCanvasGroup.DOFade(0, _interactionButtonFadeTime).OnComplete(() => im_interactionButton.enabled = false);

    }

    #region Control Panels

    public void ControlPausePanel(bool state)
    {
        if (state)
            Helpers.FadeInPanel(_pausePanel);
        else
            Helpers.FadeOutPanel(_pausePanel);
    }

    public void ControlEndPanel(bool state)
    {
        if (state)
            Helpers.FadeInPanel(_endPanel);
        else
        {
            _audioManager.PlayCrossFade("menumusic");
            _blackScreenController.FadeOutScene("Menu");
        }    
    }

    #endregion

    #region Set
    public void SetActiveCamera(GameObject camera)
    {
        _activeCamera = camera;
    }
    #endregion

    #region Get
    public GameObject GetActiveCamera()
    {
        return _activeCamera;
    }
    #endregion 
}
