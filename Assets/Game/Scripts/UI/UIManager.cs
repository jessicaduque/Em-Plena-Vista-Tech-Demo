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
    [SerializeField] CanvasGroup _indicationResetCanvasGroup;
    private CanvasGroup _interactionButtonCanvasGroup; // Canvas group for the HUD interaction button indication
    private float _hudButtonsFadeTime = 0.5f; // Time for fade in and out of interaction button indication
    private Tweener _interactionButtonTweener; // Tweener to save animation for interaction button indication
    private GameObject _activeCamera; // Active camera in scene

    [SerializeField] private Image im_interactionButton; // Interaction button Image for indication
    [SerializeField] private Sprite _interactionButtonGamepad;  // Interaction button sprite for the gamepad button
    [SerializeField] private Sprite _interactionButtonKeyboardMouse; // Interaction button sprite for the keyboard/mouse button
    [SerializeField] private Image im_indicationResetButton; // Interaction button Image for indication
    [SerializeField] private Sprite _indicationResetButtonGamepad;  
    [SerializeField] private Sprite _indicationResetButtonKeyboardMouse;

    [SerializeField] private GameObject _pausePanel; // Pause panel for control
    [SerializeField] private GameObject _endPanel; // End panel for conttrol
    [SerializeField] private Button b_exitGame; // Exit to go to menu button on the end panel

    private BlackScreenController _blackScreenController => BlackScreenController.I;
    private AudioManager _audioManager => AudioManager.I;
    private new void Awake()
    {
        _interactionButtonCanvasGroup = im_interactionButton.GetComponent<CanvasGroup>();
        im_interactionButton.sprite = (Gamepad.all.Count > 0 ? _interactionButtonGamepad : _interactionButtonKeyboardMouse);
        im_indicationResetButton.sprite = (Gamepad.all.Count > 0 ? _indicationResetButtonGamepad : _indicationResetButtonKeyboardMouse);
    }

    private void Start()
    {
        GameController.I._gamepadConnectedEvent += () => ChangeButtonsSpritesInput(true);
        GameController.I._gamepadDisconnectedEvent += () => ChangeButtonsSpritesInput(false);
    }

    private void ChangeButtonsSpritesInput(bool state)
    {
        im_interactionButton.sprite = (state ? _interactionButtonGamepad : _interactionButtonKeyboardMouse);
        im_indicationResetButton.sprite = (state ? _indicationResetButtonGamepad : _indicationResetButtonKeyboardMouse);
    }
    public void ControlInteractionButton(bool state)
    {
        _interactionButtonTweener?.Kill();

        if (state)
        {
            _interactionButtonCanvasGroup.alpha = 0;
            im_interactionButton.enabled = true;
            _interactionButtonTweener = _interactionButtonCanvasGroup.DOFade(1, _hudButtonsFadeTime).SetEase(Ease.InOutSine).OnComplete(() => _interactionButtonCanvasGroup.DOFade(0, _hudButtonsFadeTime)).SetLoops(-1, LoopType.Yoyo);
        }
        else
            _interactionButtonTweener = _interactionButtonCanvasGroup.DOFade(0, _hudButtonsFadeTime).OnComplete(() => im_interactionButton.enabled = false);

    }

    public void ControlIndicationResetInfo(bool state)
    {
        _indicationResetCanvasGroup.DOKill();
        _indicationResetCanvasGroup.DOFade(state ? 1 : 0, _hudButtonsFadeTime);
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
