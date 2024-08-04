using Utils.Singleton;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Overall controller instance for the whole tech demo
/// </summary>
public class GameController : Singleton<GameController>
{
    public event Action _gamepadConnectedEvent; // Event for when gamepad is connected
    public event Action _gamepadDisconnectedEvent; // Event for when gamepad is disconnected
    public bool gameCompleted { get; private set; } // Indicates if all puzzles have been completed and player has reached the end cave

    /// <summary>
    /// Subscribes onInputDeviceChange() to the method that recognizes when the device being used changes 
    /// </summary>
    private void Start()
    {
        InputSystem.onDeviceChange += onInputDeviceChange;
    }

    #region Devices
    /// <summary>
    /// When main device connected is changed, the adequate event will be triggered to 
    /// make any game aspect apropriate for it
    /// </summary>
    /// <param name="device">Specifies that InputDevice type object </param>
    /// <param name="change">Enum to indicate what kind of change happened</param>
    public void onInputDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                _gamepadConnectedEvent?.Invoke();
                break;
            case InputDeviceChange.Removed:
                _gamepadDisconnectedEvent?.Invoke();
                break;
        }
    }
    #endregion

}
