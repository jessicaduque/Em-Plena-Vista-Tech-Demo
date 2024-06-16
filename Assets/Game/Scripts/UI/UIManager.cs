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
    private float _interactionButtonFadeTime = 0.3f; // Time for fade in and out of interaction button indication
    private Tweener _interactionButtonTweener; // Tweener to save animation for interaction button indication
    private GameObject _activeCamera; // Active camera in scene

    [SerializeField] private Image _interactionButtonImage; // Interaction button Image for indication
    [SerializeField] private Sprite _interactionButtonGamepad;  // Interaction button sprite for the gamepad button
    [SerializeField] private Sprite _interactionButtonKeyboardMouse; // Interaction button sprite for the keyboard/mouse button
    private new void Awake()
    {
        _interactionButtonCanvasGroup = _interactionButtonImage.GetComponent<CanvasGroup>();
        _activeCamera = Helpers.cam.gameObject;
        _interactionButtonImage.sprite = (Gamepad.all.Count > 0 ? _interactionButtonGamepad : _interactionButtonKeyboardMouse);
    }

    public void ControlInteractionButton(bool state)
    {
        _interactionButtonTweener?.Kill();

        if (state)
        {
            _interactionButtonCanvasGroup.alpha = 0;
            _interactionButtonImage.enabled = true;
            _interactionButtonTweener = _interactionButtonCanvasGroup.DOFade(1, _interactionButtonFadeTime).SetEase(Ease.InOutSine).OnComplete(() => _interactionButtonCanvasGroup.DOFade(0, _interactionButtonFadeTime)).SetLoops(-1, LoopType.Yoyo);
        }
        else
            _interactionButtonTweener = _interactionButtonCanvasGroup.DOFade(0, _interactionButtonFadeTime).OnComplete(() => _interactionButtonImage.enabled = false);

    }

    #region Control Panels

    public void ControlEndPanel(bool state)
    {
        // Nothing yet
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
