using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils.Singleton;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Image _interactionButtonImage;
    [SerializeField] private Sprite _interactionButtonGamepad;
    [SerializeField] private Sprite _interactionButtonKeyboardMouse;

    private new void Awake()
    {
        _interactionButtonImage.sprite = (Gamepad.all.Count > 0 ? _interactionButtonGamepad : _interactionButtonKeyboardMouse);
    }

    public void ControlInteractionButton(bool state)
    {
        _interactionButtonImage.enabled = state;
    }
}
