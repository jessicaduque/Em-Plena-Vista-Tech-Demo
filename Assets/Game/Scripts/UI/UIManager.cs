using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils.Singleton;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    private CanvasGroup _interactionButtonCanvasGroup;
    private float _interactionButtonFadeTime = 0.3f;
    private Tweener _interactionButtonTweener;
    private GameObject _activeCamera;

    [SerializeField] private Image _interactionButtonImage;
    [SerializeField] private Sprite _interactionButtonGamepad;
    [SerializeField] private Sprite _interactionButtonKeyboardMouse;
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
