using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] private Button b_exitCredits;
    private UIControls _uiControls; // Input asset for UI controls

    private MenuUIManager _menuUIManager => MenuUIManager.I;
    private void Awake()
    {
        _uiControls = new UIControls();
    }

    private void Start()
    {
        b_exitCredits.onClick.AddListener(ControlCreditsPanel);
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
        _uiControls.InMenu.ExitMenu.started += ControlCreditsPanel;

        _uiControls.InMenu.Enable();
    }

    private void DisableInput()
    {
        _uiControls.InMenu.ExitMenu.started -= ControlCreditsPanel;

        _uiControls.InMenu.Disable();
    }

    #endregion

    public void ControlCreditsPanel()
    {
        ButtonsActivationControl(false);
        DisableInput();
        _menuUIManager.ControlCreditsPanel(false);
    }
    public void ControlCreditsPanel(InputAction.CallbackContext obj)
    {
        ControlCreditsPanel();
    }
    private void ButtonsActivationControl(bool state)
    {
        b_exitCredits.enabled = state;
    }
}
