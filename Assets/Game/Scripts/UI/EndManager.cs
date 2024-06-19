using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EndManager : MonoBehaviour
{
    [SerializeField] private Button b_exit;
    private UIControls _uiControls; // Input asset for UI controls

    private UIManager _uiManager => UIManager.I;
    private void Awake()
    {
        _uiControls = new UIControls();
    }

    private void Start()
    {
        b_exit.onClick.AddListener(ControlEndPanel);
    }

    private void OnEnable()
    {
        ButtonsActivationControl(false);
        StartCoroutine(EnableInputCooldowns(Helpers.blackFadeTime));
    }

    private void OnDisable()
    {
        DisableInput();
        StopAllCoroutines();
    }

    private IEnumerator EnableInputCooldowns(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        ButtonsActivationControl(true);
        EnableInput();
    }

    #region Input

    private void EnableInput()
    {
        _uiControls.InMenu.ExitMenu.started += ControlEndPanel;

        _uiControls.InMenu.Enable();
    }

    private void DisableInput()
    {
        _uiControls.InMenu.ExitMenu.started -= ControlEndPanel;

        _uiControls.InMenu.Disable();
    }

    #endregion

    public void ControlEndPanel()
    {
        ButtonsActivationControl(false);
        DisableInput();
        _uiManager.ControlEndPanel(false);
    }
    public void ControlEndPanel(InputAction.CallbackContext obj)
    {
        ControlEndPanel();
    }
    private void ButtonsActivationControl(bool state)
    {
        b_exit.enabled = state;
    }
}
