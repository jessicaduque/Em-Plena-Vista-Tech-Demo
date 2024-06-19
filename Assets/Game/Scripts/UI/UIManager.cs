using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils.Singleton;
using DG.Tweening;
using System.Collections;
/// <summary>
/// Manager instance in game main scene to control all UI aspects
/// </summary>
public class UIManager : Singleton<UIManager>
{
    private UIControls _uiControls; // Input asset for UI controls

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

    private ThirdPersonController _thirdPersonController => ThirdPersonController.I;
    private BlackScreenController _blackScreenController => BlackScreenController.I;
    private AudioManager _audioManager => AudioManager.I;
    private new void Awake()
    {
        _interactionButtonCanvasGroup = im_interactionButton.GetComponent<CanvasGroup>();
        im_interactionButton.sprite = (Gamepad.all.Count > 0 ? _interactionButtonGamepad : _interactionButtonKeyboardMouse);
        im_indicationResetButton.sprite = (Gamepad.all.Count > 0 ? _indicationResetButtonGamepad : _indicationResetButtonKeyboardMouse);

        _uiControls = new UIControls();
    }

    private void OnEnable()
    {
        StartCoroutine(EnableInputCooldowns(Helpers.blackFadeTime));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        GameController.I._gamepadConnectedEvent += () => ChangeButtonsSpritesInput(true);
        GameController.I._gamepadDisconnectedEvent += () => ChangeButtonsSpritesInput(false);
    }

    #region Input

    private void EnableInput()
    {
        _uiControls.InGame.PauseGame.started += ControlPausePanel;

        _uiControls.InGame.Enable();
    }

    private IEnumerator EnableInputCooldowns(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Time.timeScale = 1;
        EnableInput();
    }

    private void DisableInput()
    {
        _uiControls.InGame.PauseGame.started -= ControlPausePanel;

        _uiControls.InGame.Disable();
    }


    #endregion

    #region HUD button indication controls
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

    #endregion

    #region Control Panels

    public void ControlPausePanel(bool state)
    {
        if (state)
        {
            Helpers.LockMouse(false);
            DisableInput();
            Time.timeScale = 0;
            Helpers.FadeInPanel(_pausePanel);
        }
        else
        {
            Helpers.LockMouse(true);
            StartCoroutine(EnableInputCooldowns(Helpers.panelFadeTime));
            Helpers.FadeOutPanel(_pausePanel);
        }
    }

    public void ControlPausePanel(InputAction.CallbackContext obj)
    {
        ControlPausePanel(true);
    }

    public void ControlEndPanel(bool state)
    {
        if (state)
            _blackScreenController.FadePanel(_endPanel, true);
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
