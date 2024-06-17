using Utils.Singleton;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Overall controller instance for the whole tech demo
/// </summary>
public class GameController : Singleton<GameController>
{
    public event Action _gamepadConnected; // Event for when gamepad is connected
    public event Action _gamepadDisconnected; // Event for when gamepad is disconnected
    public bool gameCompleted { get; private set; } // Indicates if all puzzles have been completed and player has reached the end cave

    private void Start()
    {
        InputSystem.onDeviceChange += onInputDeviceChange;
    }

    #region Devices
    public void onInputDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                _gamepadConnected?.Invoke();
                break;
            case InputDeviceChange.Removed:
                _gamepadDisconnected?.Invoke();
                break;
        }
    }
    #endregion

}
