using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreditsUI : MonoBehaviour
{
    private UIControls _uiControls; // Input asset for UI controls

    private MenuUIManager _menuUIManager => MenuUIManager.I;
    private void Awake()
    {
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

    private IEnumerator EnableInputCooldowns(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
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

    public void ControlCreditsPanel(InputAction.CallbackContext obj)
    {
        DisableInput();
        _menuUIManager.ControlCreditsPanel(false);
    }
}
